// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Deferred.cginc
/// @brief Deferred shader uber-header.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_FRAMEWORK_DEFERRED_CGINC
#define A_FRAMEWORK_DEFERRED_CGINC

#define A_TANGENT_TO_WORLD_ON
#define A_DIRECT_ON
#define A_INDIRECT_ON
#define A_DEFERRED_SHADING_ON

#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

// Headers both for this file, and for all Definition and Feature modules.
#include "Assets/Alloy/Shaders/Config.cginc"
#include "Assets/Alloy/Shaders/Framework/Direct.cginc"
#include "Assets/Alloy/Shaders/Framework/Semantics.cginc"
#include "Assets/Alloy/Shaders/Framework/SurfaceImpl.cginc"

#include "UnityDeferredLibrary.cginc"
#include "UnityGlobalIllumination.cginc"
#include "UnityImageBasedLighting.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityStandardBRDF.cginc"
#include "UnityStandardUtils.cginc"

sampler2D _CameraGBufferTexture0;
sampler2D _CameraGBufferTexture1;
sampler2D _CameraGBufferTexture2;

/// Creates a surface description from a Unity G-Buffer.
/// @param[in,out] i    Unity deferred vertex format.
/// @return             Material surface data.
ASurface aDeferredSurface(
    inout unity_v2f_deferred i)
{
    ASurface s = aNewSurface();

    // Set vertex data.
    i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
    s.screenPosition = i.uv;
    s.screenUv = s.screenPosition.xy / s.screenPosition.w;

    // Convert G-Buffer to surface.
    float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, s.screenUv));
    half4 gbuffer0 = tex2D(_CameraGBufferTexture0, s.screenUv);
    half4 gbuffer1 = tex2D(_CameraGBufferTexture1, s.screenUv);
    half4 gbuffer2 = tex2D(_CameraGBufferTexture2, s.screenUv);
    float4 vpos = float4(i.ray * depth, 1.0f);

    s.viewDepth = vpos.z;
    s.positionWorld = mul(unity_CameraToWorld, vpos).xyz;
    s.viewDirWorld = normalize(UnityWorldSpaceViewDir(s.positionWorld));

    s.albedo = gbuffer0.rgb;
    s.specularOcclusion = gbuffer0.a;
    s.f0 = gbuffer1.rgb;
    s.roughness = 1.0h - gbuffer1.a;
    s.beckmannRoughness = aLinearToBeckmannRoughness(s.roughness);
    s.normalWorld = A_NW(s, normalize(gbuffer2.xyz * 2.0h - 1.0h));
    s.materialType = gbuffer2.w;
    aPreLighting(s);
    return s;
}

/// Creates a direct description from Unity light parameters.
/// @param  s   Material surface data.
/// @return     Direct description data.
ADirect aDeferredDirect(
    ASurface s)
{
    ADirect d = aNewDirect();
    float fadeDist = UnityDeferredComputeFadeDistance(s.positionWorld, s.viewDepth);
    float4 lightCoord = 0.0f;
    float3 lightVector = 0.0f;
    half3 lightAxis = 0.0h;
    half range = 1.0h;

    d.color = _LightColor.rgb;
    	
#ifndef DIRECTIONAL
    lightCoord = mul(unity_WorldToLight, float4(s.positionWorld, 1.0f));
#endif

#if defined(USING_DIRECTIONAL_LIGHT)
    lightVector = -_LightDir.xyz;
    d.shadow = UnityDeferredComputeShadow(s.positionWorld, fadeDist, s.screenUv);
        
    #if !defined(ALLOY_SUPPORT_REDLIGHTS) && defined(DIRECTIONAL_COOKIE)
        aLightCookie(d, tex2Dbias(_LightTexture0, float4(lightCoord.xy, 0, -8)));
    #endif
#elif defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
    lightVector = _LightPos.xyz - s.positionWorld;
    lightAxis = normalize(unity_WorldToLight[1].xyz);
    range = rsqrt(_LightPos.w); // _LightPos.w = 1/r*r

    #if defined(SPOT)
        // negative bias because http://aras-p.info/blog/2010/01/07/screenspace-vs-mip-mapping/
        half4 cookie = tex2Dbias(_LightTexture0, float4(lightCoord.xy / lightCoord.w, 0, -8));
        
        cookie.a *= (lightCoord.w < 0.0f);
        aLightCookie(d, cookie);
        d.shadow = UnityDeferredComputeShadow(s.positionWorld, fadeDist, s.screenUv);
    #elif defined(POINT) || defined(POINT_COOKIE)
        d.shadow = UnityDeferredComputeShadow(-lightVector, fadeDist, s.screenUv);
                
        #if defined (POINT_COOKIE)
            aLightCookie(d, texCUBEbias(_LightTexture0, float4(lightCoord.xyz, -8)));
        #endif //POINT_COOKIE
    #endif //POINT || POINT_COOKIE

    A_UNITY_ATTENUATION(d, _LightTextureB0, lightVector, _LightPos.w)
#endif

#if !defined(ALLOY_SUPPORT_REDLIGHTS) || !defined(DIRECTIONAL_COOKIE)
    aAreaLight(d, s, _LightColor, lightAxis, lightVector, range);
#else
    d.direction = lightVector;
    d.color *= redLightFunctionLegacy(_LightTexture0, s.positionWorld, s.normalWorld, s.viewDirWorld, d.direction);
    aDirectionalLight(d, s);
#endif

    return d;
}

#endif // A_FRAMEWORK_DEFERRED_CGINC
