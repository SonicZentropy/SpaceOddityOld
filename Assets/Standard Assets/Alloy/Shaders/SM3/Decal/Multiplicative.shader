﻿// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

Shader "Alloy/Decal/Multiplicative" {
Properties {
    // Main Textures
    _MainTextures ("'Main Textures' {Section:{Color:0}}", Float) = 0
    [LM_Albedo] [LM_Transparency] 
    _Color ("'Tint' {}", Color) = (1,1,1,1)	
    [LM_MasterTilingOffset] [LM_Albedo] 
    _MainTex ("'Base Color(RGB) Opacity(A)' {Visualize:{RGB}}", 2D) = "white" {}
    _MainTexVelocity ("Scroll", Vector) = (0,0,0,0) 
    _MainTexUV ("UV Set", Float) = 0
    _BaseColorVertexTint ("'Vertex Color Tint' {Min:0, Max:1}", Float) = 0
        
    // Team Color
    [Toggle(_TEAMCOLOR_ON)] 
    _TeamColor ("'Team Color' {Feature:{Color:8}}", Float) = 0
    [Enum(Masks, 0, Tint, 1)]
    _TeamColorMasksAsTint ("'Texture Mode' {Dropdown:{Masks:{}, Tint:{_TeamColorMasks, _TeamColor0, _TeamColor1, _TeamColor2, _TeamColor3}}}", Float) = 0
    _TeamColorMaskMap ("'Masks(RGBA)' {Visualize:{R, G, B, A, RGB}, Parent:_MainTex}", 2D) = "black" {}
    _TeamColorMasks ("'Channels' {Vector:Channels}", Vector) = (1,1,1,0)
    _TeamColor0 ("'Tint R' {}", Color) = (1,0,0)
    _TeamColor1 ("'Tint G' {}", Color) = (0,1,0)
    _TeamColor2 ("'Tint B' {}", Color) = (0,0,1)
    _TeamColor3 ("'Tint A' {}", Color) = (0.5,0.5,0.5)
}
SubShader {
    Tags { 
        "Queue" = "AlphaTest" 
        "IgnoreProjector" = "True" 
        "RenderType" = "Opaque" 
        "ForceNoShadowCasting" = "True" 
        //"DisableBatching" = "LODFading"
    }
    LOD 300
    Offset -1,-1

    Pass {
        Name "FORWARD" 
        Tags { "LightMode" = "ForwardBase" }

        Blend DstColor Zero
        ZWrite Off
        Cull Back

        CGPROGRAM
        #pragma target 3.0
        #pragma exclude_renderers gles
        
        #pragma shader_feature _TEAMCOLOR_ON
        
        //#pragma multi_compile __ LOD_FADE_PERCENTAGE LOD_FADE_CROSSFADE
        #pragma multi_compile_fwdbase
        #pragma multi_compile_fog
        //#pragma multi_compile_instancing
            
        #pragma vertex aVertexShader
        #pragma fragment aFragmentShader
        
        #define UNITY_PASS_FORWARDBASE
        
        #include "Assets/Alloy/Shaders/Definitions/DecalMultiplicative.cginc"
        #include "Assets/Alloy/Shaders/Passes/ForwardBase.cginc"

        ENDCG
    }
    
    Pass {
        Name "DEFERRED"
        Tags { "LightMode" = "Deferred" }

        Blend DstColor Zero
        ZWrite Off
        Cull Back

        CGPROGRAM
        #pragma target 3.0
        #pragma exclude_renderers nomrt gles

        #pragma shader_feature _TEAMCOLOR_ON
        
        //#pragma multi_compile __ LOD_FADE_PERCENTAGE LOD_FADE_CROSSFADE
        #pragma multi_compile ___ UNITY_HDR_ON
        //#pragma multi_compile_instancing
        
        #pragma vertex aVertexShader
        #pragma fragment aFragmentShader
        
        #define UNITY_PASS_DEFERRED
        
        #include "Assets/Alloy/Shaders/Definitions/DecalMultiplicative.cginc"
        #include "Assets/Alloy/Shaders/Passes/Deferred.cginc"

        ENDCG
    }
} 

FallBack Off
CustomEditor "AlloyFieldBasedEditor"
}
