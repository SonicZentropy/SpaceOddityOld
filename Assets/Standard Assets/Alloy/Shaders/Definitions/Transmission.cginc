// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Transmission.cginc
/// @brief Transmission surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_TRANSMISSION_CGINC
#define A_DEFINITIONS_TRANSMISSION_CGINC

#define A_MAIN_TEXTURES_ON
#define A_TRANSMISSION_ON

#include "Assets/Alloy/Shaders/Models/Standard.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{	
    aParallax(s);
    aDissolve(s);
    aMainTextures(s);
    aTransmission(s);
    aDetail(s);	
    aTeamColor(s);
    aAo2(s);
    aDecal(s);
    aWetness(s);
    aTwoSided(s);
    aRim(s);
    aEmission(s);
}

#endif // A_DEFINITIONS_TRANSMISSION_CGINC
