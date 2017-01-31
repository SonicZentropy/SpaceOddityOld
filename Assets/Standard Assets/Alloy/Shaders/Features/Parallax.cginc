// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Parallax.cginc
/// @brief Surface heightmap-based texcoord modification.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_FEATURES_PARALLAX_CGINC
#define A_FEATURES_PARALLAX_CGINC

#ifdef A_PARALLAX_ON
    #ifndef A_VIEW_DIR_TANGENT_ON
        #define A_VIEW_DIR_TANGENT_ON
    #endif
#endif

#include "Assets/Alloy/Shaders/Framework/Feature.cginc"
    
#ifdef A_PARALLAX_ON    
    /// Number of samples used for direct view of POM effect.
    /// Expects values in the range [1,n].
    float _MinSamples;
    
    /// Number of samples used for grazing view of POM effect.
    /// Expects values in the range [1,n].
    float _MaxSamples;
#endif

void aParallax(
    inout ASurface s) 
{
#ifdef A_PARALLAX_ON
    #ifndef _BUMPMODE_POM
        aOffsetBumpMapping(s);
    #else
        aParallaxOcclusionMapping(s, _MinSamples, _MaxSamples);
    #endif 
#endif 
}

#endif // A_FEATURES_PARALLAX_CGINC