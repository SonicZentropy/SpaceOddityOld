// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Transition.cginc
/// @brief Transition & Weighted Blend shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_TRANSITION_CGINC
#define A_DEFINITIONS_TRANSITION_CGINC

#define A_MAIN_TEXTURES_ON
#define A_MAIN_TEXTURES_CUTOUT_OFF
#define A_TRANSITION_BLEND_ON
#define A_SECONDARY_TEXTURES_ON
#define A_SECONDARY_TEXTURES_ALPHA_BLEND_OFF

#include "Assets/Alloy/Shaders/Models/Standard.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{
    aTransitionBlend(s);

    s.mask = 1.0h - s.mask;
    aParallax(s);
    aMainTextures(s);

    s.mask = 1.0h - s.mask;
    aSecondaryTextures(s);
    aCutout(s);

    s.mask = 1.0h - s.mask;
    aDetail(s);
    aTeamColor(s);
    aDecal(s);
    aWetness(s);
    aEmission(s);
    aRim(s);
    
    s.mask = 1.0h;
    aDissolve(s);
    aAo2(s);
}

#endif // A_DEFINITIONS_TRANSITION_CGINC
