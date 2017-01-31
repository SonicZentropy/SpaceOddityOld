// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

Shader "Hidden/Alloy/Deferred Reflections" {
Properties {
    _SrcBlend ("", Float) = 1
    _DstBlend ("", Float) = 1
}
SubShader {

// Calculates reflection contribution from a single probe (rendered as cubes) or default reflection (rendered as full screen quad)
Pass {
    ZWrite Off
    ZTest LEqual
    Blend [_SrcBlend] [_DstBlend]
CGPROGRAM
#pragma target 3.0
#pragma vertex vert_deferred
#pragma fragment frag

#include "Assets/Alloy/Shaders/Framework/Deferred.cginc"

half3 distanceFromAABB(half3 p, half3 aabbMin, half3 aabbMax)
{
    return max(max(p - aabbMax, aabbMin - p), half3(0.0, 0.0, 0.0));
}

half4 frag (unity_v2f_deferred i) : SV_Target
{
    UnityGIInput d;
    ASurface s = aDeferredSurface(i);
    float blendDistance = unity_SpecCube1_ProbePosition.w; // will be set to blend distance for this probe

	d.worldPos = s.positionWorld;
	d.worldViewDir = s.viewDirWorld; // ???
	d.probeHDR[0] = unity_SpecCube0_HDR;

#if UNITY_SPECCUBE_BOX_PROJECTION
	d.probePosition[0]	= unity_SpecCube0_ProbePosition;
	d.boxMin[0].xyz		= unity_SpecCube0_BoxMin - float4(blendDistance,blendDistance,blendDistance,0);
	d.boxMin[0].w		= 1;  // 1 in .w allow to disable blending in UnityGI_IndirectSpecular call
	d.boxMax[0].xyz		= unity_SpecCube0_BoxMax + float4(blendDistance,blendDistance,blendDistance,0);
#endif

    Unity_GlossyEnvironmentData g;
    AIndirect ind = aNewIndirect();

    g.roughness = s.roughness;
    g.reflUVW = s.reflectionVectorWorld;
    ind.specular = UnityGI_IndirectSpecular(d, 1.0h, g);

    // Calculate falloff value, so reflections on the edges of the probe would gradually blend to previous reflection.
    // Also this ensures that pixels not located in the reflection probe AABB won't
    // accidentally pick up reflections from this probe.
    half3 distance = distanceFromAABB(s.positionWorld, unity_SpecCube0_BoxMin.xyz, unity_SpecCube0_BoxMax.xyz);
    half falloff = saturate(1.0 - length(distance) / blendDistance);

    return half4(aIndirect(ind, s), falloff);
}

ENDCG
}

// Adds reflection buffer to the lighting buffer
Pass
{
	ZWrite Off
	ZTest Always
	Blend [_SrcBlend] [_DstBlend]

	CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile ___ UNITY_HDR_ON

		#include "UnityCG.cginc"

		sampler2D _CameraReflectionsTexture;

		struct v2f {
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
		};

		v2f vert (float3 vertex : POSITION)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(vertex);
			o.uv = ComputeScreenPos (o.pos).xy;
			return o;
		}

		half4 frag (v2f i) : SV_Target
		{
			half4 c = tex2D (_CameraReflectionsTexture, i.uv);
			#ifdef UNITY_HDR_ON
			return float4(c.rgb, 0.0f);
			#else
			return float4(exp2(-c.rgb), 0.0f);
			#endif

		}
	ENDCG
}

}
Fallback Off
}

