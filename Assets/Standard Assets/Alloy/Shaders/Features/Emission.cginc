// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Emission.cginc
/// @brief Surface emission effects.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_FEATURES_EMISSION_CGINC
#define A_FEATURES_EMISSION_CGINC

#ifdef A_EMISSION_ON
    #ifndef A_EMISSIVE_COLOR_ON
        #define A_EMISSIVE_COLOR_ON
    #endif
#endif

#include "Assets/Alloy/Shaders/Framework/Feature.cginc"

#ifdef A_EMISSION_ON
    /// Emission tint color.
    /// Expects a linear LDR color.
    half3 _EmissionColor;

    #ifndef A_EMISSION_MASK_MAP_OFF
        /// Emission mask texture.
        /// Expects an RGB map with sRGB sampling.
        sampler2D _EmissionMap;
    #endif

    #ifndef A_EMISSION_EFFECTS_MAP_OFF
        /// Emission effect texture.
        /// Expects an RGB map with sRGB sampling.
        A_SAMPLER_2D(_IncandescenceMap);
    #endif
    
    /// The weight of the emission effect.
    /// Expects linear space value in the range [0,1].
    half _EmissionWeight;
#endif

void aEmission(
    inout ASurface s)
{
#ifdef A_EMISSION_ON 
    half3 emission = _EmissionColor;

    #ifndef A_EMISSION_MASK_MAP_OFF
        emission *= tex2D(_EmissionMap, s.baseUv).rgb;
    #endif

    #ifndef A_EMISSION_EFFECTS_MAP_OFF
        float2 incandescenceUv = A_TEX_TRANSFORM_UV_SCROLL(s, _IncandescenceMap);
        emission *= tex2D(_IncandescenceMap, incandescenceUv).rgb;
    #endif
    
    s.emissiveColor += emission * (_EmissionWeight * s.mask);
#endif
} 

#endif // A_FEATURES_EMISSION_CGINC
