// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

///////////////////////////////////////////////////////////////////////////////
/// @file Pass.cginc
/// @brief Passes uber-header.
///////////////////////////////////////////////////////////////////////////////

#ifndef A_FRAMEWORK_PASS_CGINC
#define A_FRAMEWORK_PASS_CGINC

// Headers both for this file, and for all Definition and Feature modules.
#include "Assets/Alloy/Shaders/Framework/FeatureImpl.cginc"
#include "Assets/Alloy/Shaders/Framework/Keywords.cginc"
#include "Assets/Alloy/Shaders/Framework/ModelImpl.cginc"
#include "Assets/Alloy/Shaders/Framework/SplatImpl.cginc"
#include "Assets/Alloy/Shaders/Framework/SurfaceImpl.cginc"
#include "Assets/Alloy/Shaders/Framework/Tessellation.cginc"
#include "Assets/Alloy/Shaders/Framework/Unity.cginc"
#include "Assets/Alloy/Shaders/Framework/Utility.cginc"

#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "UnityGlobalIllumination.cginc"
#include "UnityInstancing.cginc"
#include "UnityLightingCommon.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityStandardBRDF.cginc"
#include "UnityStandardUtils.cginc"

#ifdef A_TWO_SIDED_ON
    #define A_FACING_TYPE ,half facingSign : VFACE
    #define A_FACING_SIGN facingSign
#else
    #define A_FACING_TYPE
    #define A_FACING_SIGN 1.0h
#endif

/// Transfers the per-vertex surface data to the pixel shader.
/// @param[in,out]  v       Vertex input data.
/// @param[out]     o       Vertex to fragment transfer data.
/// @param[out]     opos    Clip space position.
void aTransferVertex(
    inout AVertex v,
    out AVertexToFragment o, 
    out float4 opos)
{
#ifdef A_SURFACE_SHADER_OFF
    opos = 0.0h;
#else
    UNITY_INITIALIZE_OUTPUT(AVertexToFragment, o);

    #ifdef A_INSTANCING_PASS
        UNITY_SETUP_INSTANCE_ID(v);

        #ifdef A_TRANSFER_INSTANCE_ID_ON
            UNITY_TRANSFER_INSTANCE_ID(v, o);
        #endif

        #ifdef A_STEREO_PASS
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        #endif
    #endif

    aVertex(v);

    o.color = v.color; // Gamma-space vertex color, unless modified.
    o.texcoords.xy = v.uv0.xy;
    o.texcoords.zw = v.uv1.xy;
    opos = UnityObjectToClipPos(v.vertex.xyz);
    
    #ifdef A_POSITION_TEXCOORD_ON
        #ifdef A_POSITION_WORLD_ON
            o.positionWorldAndViewDepth.xyz = mul(unity_ObjectToWorld, v.vertex).xyz;
        #endif

        #ifdef A_VIEW_DEPTH_ON
            COMPUTE_EYEDEPTH(o.positionWorldAndViewDepth.w);
        #endif
    #endif
        
    #ifdef A_NORMAL_WORLD_ON
        float3 normalWorld = UnityObjectToWorldNormal(v.normal);
    #endif

    #ifndef A_TANGENT_TO_WORLD_ON
        #ifdef A_NORMAL_WORLD_ON
            o.normalWorld = normalWorld;
        #endif
    #else
        float3 tangentWorld = UnityObjectToWorldDir(v.tangent.xyz);
        float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld, v.tangent.w);

        o.tangentWorld = tangentToWorld[0];
        o.bitangentWorld = tangentToWorld[1];
        o.normalWorld = tangentToWorld[2];
    #endif
        
    #ifdef A_FOG_TEXCOORD_ON
        #if defined(A_SCREEN_UV_ON) || defined(A_COMPUTE_VERTEX_SCREEN_UV)
            o.fogCoord.yzw = ComputeScreenPos(opos).xyw;
        #else
            o.fogCoord.yzw = A_AXIS_Z;
        #endif

        UNITY_TRANSFER_FOG(o, opos);
    #endif
#endif
}

/// Create a ASurface populated with data from the vertex shader.
/// @param  i           Vertex to fragment transfer data.
/// @param  facingSign  Sign of front/back facing direction.
/// @return             Initialized surface data object.
ASurface aForwardSurface(
    AVertexToFragment i,
    half facingSign)
{
    ASurface s = aNewSurface();

#ifndef A_SURFACE_SHADER_OFF
    #ifdef A_TRANSFER_INSTANCE_ID_ON
        UNITY_SETUP_INSTANCE_ID(i);
    #endif

    s.uv01 = i.texcoords;
    s.vertexColor = i.color;
    s.facingSign = facingSign;

    #ifdef A_POSITION_TEXCOORD_ON
        #ifdef A_POSITION_WORLD_ON
            s.positionWorld = i.positionWorldAndViewDepth.xyz;
        #endif
        
        #ifdef A_VIEW_DEPTH_ON
            s.viewDepth = i.positionWorldAndViewDepth.w;
        #endif
    #endif

    #ifdef A_NORMAL_WORLD_ON
        #ifdef A_TWO_SIDED_ON
            i.normalWorld.xyz *= facingSign;
        #endif

        // Give these sane defaults in case the surface shader doesn't set them.
        s.vertexNormalWorld = normalize(i.normalWorld);
        s.normalWorld = s.vertexNormalWorld;
        s.ambientNormalWorld = s.vertexNormalWorld;
    #endif

    #ifdef A_VIEW_DIR_WORLD_ON
        // Cheaper to calculate in PS than to unpack from vertex, while also
        // preventing distortion in POM and area light specular highlights.
        s.viewDirWorld = normalize(UnityWorldSpaceViewDir(s.positionWorld));
    #endif

    #ifdef A_TANGENT_TO_WORLD_ON
        half3 t = i.tangentWorld;
        half3 b = i.bitangentWorld;
        half3 n = i.normalWorld;
        
        #if UNITY_TANGENT_ORTHONORMALIZE
            n = normalize(n);
    
            // ortho-normalize Tangent
            t = normalize (t - n * dot(t, n));

            // recalculate Binormal
            half3 newB = cross(n, t);
            b = newB * sign (dot (newB, b));
        #endif

        s.tangentToWorld = half3x3(t, b, n);

        #if defined(A_VIEW_DIR_WORLD_ON) && defined(A_VIEW_DIR_TANGENT_ON)
            s.viewDirTangent = normalize(mul(s.tangentToWorld, s.viewDirWorld));
        #endif
    #endif
        
    #ifdef A_FOG_TEXCOORD_ON
        #ifdef A_FOG_ON
            s.fogCoord = i.fogCoord;
        #endif

        #ifdef A_SCREEN_UV_ON
            s.screenPosition.xyw = i.fogCoord.yzw;
            s.screenPosition.z = 0.0h;
            s.screenUv.xy = s.screenPosition.xy / s.screenPosition.w;

            #ifdef LOD_FADE_CROSSFADE
                half2 projUV = s.screenUv.xy * _ScreenParams.xy * 0.25h;

                projUV.y = frac(projUV.y) * 0.0625h /* 1/16 */ + unity_LODFade.y; // quantized lod fade by 16 levels
                clip(tex2D(_DitherMaskLOD2D, projUV).a - 0.5f);
            #endif
        #endif
    #endif

    // Runs the shader and lighting type's surface code.
    aBaseUvInit(s);
    aUpdateViewData(s);

    aSurface(s);

    #ifndef A_LIGHTING_ON
        // Unlit (kill everything except normals and emission).
        s.albedo = 0.0h;
        s.specularOcclusion = 0.0h;
        s.f0 = 0.0h;
        s.roughness = 1.0h;
        s.materialType = 1.0h;
        s.subsurface = 0.0h;
    #else
        s.baseColor = saturate(s.baseColor);
        s.albedo = s.baseColor;
        s.f0 = aSpecularityToF0(s.specularity);

        #ifdef A_SPECULAR_TINT_ON
            s.f0 *= aLerpWhiteTo(aChromaticity(s.baseColor), s.specularTint);
        #endif

        #ifdef A_METALLIC_ON
            half metallicInv = 1.0h - s.metallic;

            s.albedo *= metallicInv; // Affects transmission through albedo.
            s.f0 = lerp(s.f0, s.baseColor, s.metallic);

            #ifdef _ALPHAPREMULTIPLY_ON
                // Interpolate from a translucent dielectric to an opaque metal.
                s.opacity = s.metallic + metallicInv * s.opacity;
            #endif
        #endif

        #ifdef A_CLEARCOAT_ON
            // f0 of 0.04 gives us a polyurethane-like coating.
            half clearCoat = s.clearCoat * lerp(0.04h, 1.0h, s.FV);

            s.albedo *= aLerpOneTo(0.0h, clearCoat);
            s.f0 = lerp(s.f0, A_WHITE, clearCoat);
            s.roughness = lerp(s.roughness, s.clearCoatRoughness, clearCoat);
        #endif

        #ifdef _ALPHAPREMULTIPLY_ON
            // Premultiply opacity with albedo for translucent shaders.
            s.albedo *= s.opacity;
        #endif

        s.beckmannRoughness = aLinearToBeckmannRoughness(s.roughness);

        #ifndef A_AMBIENT_OCCLUSION_ON
            s.specularOcclusion = 1.0h;
        #else
            s.specularOcclusion = aSpecularOcclusion(s.ambientOcclusion, s.NdotV);
        #endif

        aPreLighting(s);
    #endif
#endif

    return s;
}

/// Transfers the per-vertex lightmapping or SH data to the fragment shader.
/// @param[in,out]  i   Vertex to fragment transfer data.
/// @param[in]      v   Vertex input data.
void aVertexGi(
    inout AVertexToFragment i,
    AVertex v)
{
#ifdef A_INDIRECT_ON
    #ifdef LIGHTMAP_ON
        i.giData.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
        i.giData.zw = 0.0h;
    #elif UNITY_SHOULD_SAMPLE_SH
        // Add approximated illumination from non-important point lights
        half3 normalWorld = i.normalWorld.xyz;

        #ifdef VERTEXLIGHT_ON
            i.giData.rgb = aShade4PointLights(i.positionWorldAndViewDepth.xyz, normalWorld);
        #endif

        i.giData.rgb = ShadeSHPerVertex(normalWorld, i.giData.rgb);
    #endif

    #ifdef DYNAMICLIGHTMAP_ON
        i.giData.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif
#endif
}

/// Populates a UnityGI descriptor in the fragment shader.
/// @param  i       Vertex to fragment transfer data.
/// @param  s       Material surface data.
/// @param  shadow  Forward Base directional light shadow.
/// @return         Initialized UnityGI descriptor.
UnityGI aFragmentGi(
    AVertexToFragment i,
    ASurface s,
    half shadow)
{
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);

#ifdef A_INDIRECT_ON
    UnityGIInput d;

    UNITY_INITIALIZE_OUTPUT(UnityGIInput, d);
    d.worldPos = s.positionWorld;
    d.worldViewDir = s.viewDirWorld; // ???
    d.atten = shadow;

    #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
        d.ambient = 0;
        d.lightmapUV = i.giData;
    #else
        d.ambient = i.giData.rgb;
        d.lightmapUV = 0;
    #endif

    d.probeHDR[0] = unity_SpecCube0_HDR;
    d.probeHDR[1] = unity_SpecCube1_HDR;

    #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
        d.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
    #endif

    #if UNITY_SPECCUBE_BOX_PROJECTION
        d.boxMax[0] = unity_SpecCube0_BoxMax;
        d.probePosition[0] = unity_SpecCube0_ProbePosition;
        d.boxMax[1] = unity_SpecCube1_BoxMax;
        d.boxMin[1] = unity_SpecCube1_BoxMin;
        d.probePosition[1] = unity_SpecCube1_ProbePosition;
    #endif

    // Pass 1.0 for occlusion so we can apply it later in indirect().  
    gi = UnityGI_Base(d, 1.0h, s.ambientNormalWorld);

    #ifdef A_REFLECTION_PROBES_ON
        Unity_GlossyEnvironmentData g;

        g.reflUVW = s.reflectionVectorWorld;
        g.roughness = s.roughness;
        gi.indirect.specular = UnityGI_IndirectSpecular(d, 1.0h, s.normalWorld, g);
    #endif
#endif

    return gi;
}

/// Final processing of the forward output.
/// @param  s       Material surface data.
/// @param  color   Lighting + Emission + Fog + etc.
/// @return         Final HDR output color with alpha opacity.
half4 aOutputForward(
    ASurface s,
    half3 color)
{
    half4 output;
    output.rgb = color;

#if defined(A_ALPHA_BLENDING_ON) && !defined(A_PASS_DISTORT)
    output.a = s.opacity;
#else
    UNITY_OPAQUE_ALPHA(output.a);
#endif

#ifdef VTRANSPARENCY_ON
    float4 data = s.screenPosition;

    data.z = s.viewDepth;

    #if defined(UNITY_PASS_FORWARDBASE) || defined(A_PASS_DISTORT)
        output = VolumetricTransparencyBase(output, data);
    #else
        output = VolumetricTransparencyAdd(output, data);
    #endif
#endif

    aFinalColor(output, s);
    return aHdrClamp(output);
}

/// Final processing of the deferred output.
/// @param  i           Vertex to fragment transfer data.
/// @param  facingSign  Sign of front/back facing direction.
/// @return             G-buffer with surface data and ambient illumination.
AGbuffer aOutputDeferred(
    AVertexToFragment i,
    half facingSign)
{
    AGbuffer gb;
    ASurface s = aForwardSurface(i, facingSign);
    UnityGI gi = aFragmentGi(i, s, 1.0h);

    UNITY_INITIALIZE_OUTPUT(AGbuffer, gb);
    gb.target3 = aUnityOutputDeferred(s, gi, gb.target0, gb.target1, gb.target2);
    
#ifndef UNITY_HDR_ON
    gb.target3.rgb = exp2(-gb.target3.rgb);
#endif

    aFinalGbuffer(gb, s);
    return gb;
}

#endif // A_FRAMEWORK_PASS_CGINC
