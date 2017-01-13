// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/Lux Pro Internal-DeferredShading" {
Properties {
	_LightTexture0 ("", any) = "" {}
	_LightTextureB0 ("", 2D) = "" {}
	_ShadowMapTexture ("", any) = "" {}
	_SrcBlend ("", Float) = 1
	_DstBlend ("", Float) = 1
}
SubShader {

// Pass 1: Lighting pass
//  LDR case - Lighting encoded into a subtractive ARGB8 buffer
//  HDR case - Lighting additively blended into floating point buffer
Pass {
	ZWrite Off
	Blend [_SrcBlend] [_DstBlend]

CGPROGRAM
#pragma target 3.0
#pragma vertex vert_deferred
#pragma fragment frag
#pragma multi_compile_lightpass
#pragma multi_compile ___ UNITY_HDR_ON

#pragma multi_compile __ LUX_AREALIGHTS

#pragma exclude_renderers nomrt

#include "UnityCG.cginc"
//#include "Lux Deferred Library.cginc"
//#ZENCHANGED HERE
// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'
// Upgrade NOTE: replaced 'unity_World2Shadow' with 'unity_WorldToShadow'

#ifndef LUX_PRO_DEFERRED_LIBRARY_INCLUDED
#define LUX_PRO_DEFERRED_LIBRARY_INCLUDED

// Deferred lighting / shading helpers


// --------------------------------------------------------
// Vertex shader

struct unity_v2f_deferred {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float3 ray : TEXCOORD1;
	};

	float _LightAsQuad;

	unity_v2f_deferred vert_deferred(float4 vertex : POSITION, float3 normal : NORMAL)
	{
		unity_v2f_deferred o;
		o.pos = mul(UNITY_MATRIX_MVP, vertex);
		o.uv = ComputeScreenPos(o.pos);
		o.ray = mul(UNITY_MATRIX_MV, vertex).xyz * float3(-1,-1,1);

		// normal contains a ray pointing from the camera to one of near plane's
		// corners in camera space when we are drawing a full screen quad.
		// Otherwise, when rendering 3D shapes, use the ray calculated here.
		o.ray = lerp(o.ray, normal, _LightAsQuad);

		return o;
	}


	// --------------------------------------------------------
	// Shared uniforms


	sampler2D_float _CameraDepthTexture;

	float4 _LightDir;
	float4 _LightPos;
	float4 _LightColor;
	float4 unity_LightmapFade;
	CBUFFER_START(UnityPerCamera2)
		// float4x4 _CameraToWorld;
		CBUFFER_END
		float4x4 unity_WorldToLight;
	sampler2D _LightTextureB0;

#if defined (POINT_COOKIE)
	samplerCUBE _LightTexture0;
#else
	sampler2D _LightTexture0;
#endif

#if defined (SHADOWS_SCREEN)
	sampler2D _ShadowMapTexture;
#endif

	float _Lux_ShadowDistance;


	// --------------------------------------------------------
	// Shadow/fade helpers

#include "UnityShadowLibrary.cginc"


	float UnityDeferredComputeFadeDistance(float3 wpos, float z)
	{
		float sphereDist = distance(wpos, unity_ShadowFadeCenterAndType.xyz);
		return lerp(z, sphereDist, unity_ShadowFadeCenterAndType.w);
	}

	half UnityDeferredComputeShadow(float3 vec, float fadeDist, float2 uv)
	{
#if defined(SHADOWS_DEPTH) || defined(SHADOWS_SCREEN) || defined(SHADOWS_CUBE)
		float fade = fadeDist * _LightShadowData.z + _LightShadowData.w;
		fade = saturate(fade);
#endif

#if defined(SPOT)
#if defined(SHADOWS_DEPTH)
		float4 shadowCoord = mul(unity_WorldToShadow[0], float4(vec,1));
		return saturate(UnitySampleShadowmap(shadowCoord) + fade);
#endif //SHADOWS_DEPTH
#endif

#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
#if defined(SHADOWS_SCREEN)
		return saturate(tex2D(_ShadowMapTexture, uv).r + fade);
#endif
#endif //DIRECTIONAL || DIRECTIONAL_COOKIE

#if defined (POINT) || defined (POINT_COOKIE)
#if defined(SHADOWS_CUBE)
		return UnitySampleShadowmap(vec);
#endif //SHADOWS_CUBE
#endif

		return 1.0;
	}


	// --------------------------------------------------------
	// Common lighting data calculation (direction, attenuation, ...)


	void LuxDeferredCalculateLightParams(
		unity_v2f_deferred i,
		out float3 outWorldPos,
		out float2 outUV,
		out half3 outLightDir,
		out float outAtten,
		out float outFadeDist,
		out float outShadow,
		out float outTransfade)
	{
		i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
		float2 uv = i.uv.xy / i.uv.w;

		// read depth and reconstruct world position
		float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
		depth = Linear01Depth(depth);
		float4 vpos = float4(i.ray * depth,1);
		float3 wpos = mul(unity_CameraToWorld, vpos).xyz;

		float fadeDist = UnityDeferredComputeFadeDistance(wpos, vpos.z);
		float shadow = 1.0;

		// spot light case
#if defined (SPOT)	
		float3 tolight = _LightPos.xyz - wpos;
		half3 lightDir = normalize(tolight);

		float4 uvCookie = mul(unity_WorldToLight, float4(wpos,1));
		// negative bias because http://aras-p.info/blog/2010/01/07/screenspace-vs-mip-mapping/
		float atten = tex2Dbias(_LightTexture0, float4(uvCookie.xy / uvCookie.w, 0, -8)).w;
		atten *= uvCookie.w < 0;
		float att = dot(tolight, tolight) * _LightPos.w;
		atten *= tex2D(_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
		//	Lux:	
		shadow = UnityDeferredComputeShadow(wpos, fadeDist, uv);

		// directional light case		
#elif defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
		half3 lightDir = -_LightDir.xyz;
		float atten = 1.0;

		shadow = UnityDeferredComputeShadow(wpos, fadeDist, uv);

#if defined (DIRECTIONAL_COOKIE)
		atten *= tex2Dbias(_LightTexture0, float4(mul(unity_WorldToLight, half4(wpos,1)).xy, 0, -8)).w;
#endif //DIRECTIONAL_COOKIE

		// point light case	
#elif defined (POINT) || defined (POINT_COOKIE)
		float3 tolight = wpos - _LightPos.xyz;
		half3 lightDir = -normalize(tolight);

		float att = dot(tolight, tolight) * _LightPos.w;
		float atten = tex2D(_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
		//	Lux:	
		shadow = UnityDeferredComputeShadow(tolight, fadeDist, uv);

#if defined (POINT_COOKIE)
		atten *= texCUBEbias(_LightTexture0, float4(mul(unity_WorldToLight, half4(wpos,1)).xyz, -8)).w;
#endif //POINT_COOKIE	
#else
		half3 lightDir = 0;
		float atten = 0;
#endif

		outWorldPos = wpos;
		outUV = uv;
		outLightDir = lightDir;
		outAtten = atten;
		outFadeDist = fadeDist;
		//	Lux: Smoothly fade out point light shadows
#if defined (POINT) || defined (POINT_COOKIE)
		outShadow = lerp(shadow, 1.0, (1.0 - saturate((_Lux_ShadowDistance - vpos.z) * 0.0375)));
#else
		outShadow = shadow;
#endif
		//	Lux: Currently not used
		outTransfade = 1; //saturate( (_Lux_ShadowDistance - vpos.z ) * 0.0375 ); // Fades out translucent lighting
	}

#endif // UNITY_DEFERRED_LIBRARY_INCLUDED

#include "UnityPBSLighting.cginc"
#include "UnityStandardUtils.cginc"
#include "UnityStandardBRDF.cginc"

#include "../Lux Lighting/LuxAreaLights.cginc"
#include "../Lux BRDFs/LuxStandardBRDF.cginc"

sampler2D _CameraGBufferTexture0;
sampler2D _CameraGBufferTexture1;
sampler2D _CameraGBufferTexture2;

half4 CalculateLight (unity_v2f_deferred i)
{
	float3 wpos;
	float2 uv;
	float atten, fadeDist, shadow, transfade;
	UnityLight light;
	UNITY_INITIALIZE_OUTPUT(UnityLight, light);
	
//	///////////////////////////////////////	
//	Lux: Light attenuation and shadow attenuation will be returned separately	
	LuxDeferredCalculateLightParams (i, wpos, uv, light.dir, atten, fadeDist, shadow, transfade);

	half4 gbuffer0 = tex2D (_CameraGBufferTexture0, uv);
	half4 gbuffer1 = tex2D (_CameraGBufferTexture1, uv);
	half4 gbuffer2 = tex2D (_CameraGBufferTexture2, uv);

	light.color = _LightColor.rgb * atten;

	half3 baseColor = gbuffer0.rgb;
	half3 specColor = gbuffer1.rgb;
	half oneMinusRoughness = gbuffer1.a;

	float3 eyeVec = normalize(wpos-_WorldSpaceCameraPos);
	half oneMinusReflectivity = 1 - SpecularStrength(specColor.rgb);
	
	UnityIndirect ind;
	UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
	ind.diffuse = 0;
	ind.specular = 0;

//	//////////////////////
//	Lux: Set up the needed variables for area lights
	half4 res = 1;
	half ndotlDiffuse = 1;
	half3 diffuseLightDir = light.dir;
	half specularIntensity = 1;
	half3 normalWorld = gbuffer2.rgb * 2 - 1;
	half3 diffuseNormalWorld = normalWorld;


//	///////////////////////////////////////	
//	Lux: Important!
	normalWorld = normalize(normalWorld); // To avoid strange lighting artifacts on very smooth surfaces
	#if !UNITY_BRDF_GGX
		light.ndotl = LambertTerm (normalWorld, light.dir);
	#endif

//	///////////////////////////////////////	
//	Lux: Area lights
	#if defined(LUX_AREALIGHTS)
		// NOTE: Deferred needs other inputs than forward
		float3 lightPos = float3(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3]);
		Lux_AreaLight (light, specularIntensity, diffuseLightDir, ndotlDiffuse, light.dir, _LightColor.a, lightPos, wpos, eyeVec, normalWorld, diffuseNormalWorld, 1.0 - oneMinusRoughness);
	#else
		light.ndotl = LambertTerm (normalWorld, light.dir);
		diffuseLightDir = light.dir;
		ndotlDiffuse = LambertTerm(diffuseNormalWorld, light.dir);
		// If area lights are disabled we still have to reduce specular intensity
		#if !defined(DIRECTIONAL) && !defined(DIRECTIONAL_COOKIE)
			specularIntensity = saturate(_LightColor.a);
		#endif
	#endif

//	///////////////////////////////////////	
//	Lux: Set up inputs shared by all BRDFs
	#define viewDir -eyeVec
	// From Unity's standard BRDF: As we declare all inputs up front we do this here as well.
	#if UNITY_BRDF_GGX 
		// NdotV should not be negative for visible pixels, but it can happen due to perspective projection and normal mapping
		// In this case we will modify the normal so it become valid and not cause weird artifact (other game try to clamp or abs the NdotV to prevent this trouble).
		// The amount we shift the normal toward the view vector is define by the dot product.
		// This correction is only apply with smithJoint visibility function because artifact are more visible in this case due to highlight edge of rough surface
		half shiftAmount = dot(normalWorld, viewDir);
		normalWorld = shiftAmount < 0.0f ? normalWorld + viewDir * (-shiftAmount + 1e-5f) : normalWorld;
		// A re-normalization should be apply here but as the shift is small we don't do it to save ALU.
		//normal = normalize(normal);
		// As we have modify the normal we need to recalculate the dot product nl. 
		// Note that  light.ndotl is a clamped cosine and only the ForwardSimple mode use a specific ndotL with BRDF3
		light.ndotl = DotClamped(normalWorld, light.dir);
	//#else
	//	half nl = light.ndotl;
	#endif

	half3 halfDir = Unity_SafeNormalize (light.dir + viewDir);
	half nh = BlinnTerm (normalWorld, halfDir);
	half nv = DotClamped (normalWorld, viewDir);
	half lv = DotClamped (light.dir, viewDir);
	half lh = DotClamped (light.dir, halfDir);

//	/////////////////////
//	Lux: Standard Lighting
	
//	Add support for "real" lambert lighting
	specularIntensity = (specColor.r == 0.0) ? 0.0 : specularIntensity;

	res = Lux_BRDF1_PBS (baseColor, specColor, oneMinusReflectivity, oneMinusRoughness, normalWorld, -eyeVec,
		  halfDir, nh, nv, lv, lh,
		  ndotlDiffuse,
	 	  light, ind,
	 	  specularIntensity,
	 	  shadow);

	return res;
}

#ifdef UNITY_HDR_ON
half4
#else
fixed4
#endif
frag (unity_v2f_deferred i) : SV_Target
{
	half4 c = CalculateLight(i);
	#ifdef UNITY_HDR_ON
	return c;
	#else
	return exp2(-c);
	#endif
}

ENDCG
}


// Pass 2: Final decode pass.
// Used only with HDR off, to decode the logarithmic buffer into the main RT
Pass {
	ZTest Always Cull Off ZWrite Off
	Stencil {
		ref [_StencilNonBackground]
		readmask [_StencilNonBackground]
		// Normally just comp would be sufficient, but there's a bug and only front face stencil state is set (case 583207)
		compback equal
		compfront equal
	}

CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers nomrt

sampler2D _LightBuffer;
struct v2f {
	float4 vertex : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

v2f vert (float4 vertex : POSITION, float2 texcoord : TEXCOORD0)
{
	v2f o;
	o.vertex = mul(UNITY_MATRIX_MVP, vertex);
	o.texcoord = texcoord.xy;
	return o;
}

fixed4 frag (v2f i) : SV_Target
{
	return -log2(tex2D(_LightBuffer, i.texcoord));
}
ENDCG 
}

}
Fallback Off
}
