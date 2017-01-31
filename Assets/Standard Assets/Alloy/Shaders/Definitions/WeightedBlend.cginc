// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file WeightedBlend.cginc
/// @brief Weighted Blend shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_WEIGHTED_BLEND_CGINC
#define A_DEFINITIONS_WEIGHTED_BLEND_CGINC

#define A_MAIN_TEXTURES_ON
#define A_MAIN_TEXTURES_CUTOUT_OFF
#define A_WEIGHTED_BLEND_ON
#define A_SECONDARY_TEXTURES_ON

#include "Assets/Alloy/Shaders/Models/Standard.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"
    
void aSurface(
    inout ASurface s)
{
    aParallax(s);
    aDissolve(s);
    aMainTextures(s);
    aDetail(s);
    aTeamColor(s);
    aEmission(s);

    aHeightmapBlend(s);
    aSecondaryTextures(s);
    aCutout(s);

    s.mask = 1.0h;
    aAo2(s);
    aDecal(s);
    aWetness(s);
    aRim(s);
}

#endif // A_DEFINITIONS_WEIGHTED_BLEND_CGINC
