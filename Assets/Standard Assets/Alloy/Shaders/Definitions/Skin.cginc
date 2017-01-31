// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Skin.cginc
/// @brief Skin surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_SKIN_CGINC
#define A_DEFINITIONS_SKIN_CGINC

#define A_SKIN_TEXTURES_ON

#include "Assets/Alloy/Shaders/Models/Standard.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{
    aParallax(s);
    aDissolve(s);
    aSkinTextures(s);
    aDetail(s);
    aTeamColor(s);
    aDecal(s);
    aWetness(s);	
    aRim(s);
    aEmission(s);
}

#endif // A_DEFINITIONS_SKIN_CGINC
