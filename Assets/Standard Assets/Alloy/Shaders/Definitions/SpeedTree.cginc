// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file SpeedTree.cginc
/// @brief SpeedTree surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_SPEED_TREE_CGINC
#define A_DEFINITIONS_SPEED_TREE_CGINC

#define A_SPEED_TREE_ON
#define A_SPECULAR_TINT_ON

#include "Assets/Alloy/Shaders/Models/SpeedTree.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{
    aParallax(s);
    aDissolve(s);
    aSpeedTree(s);
    s.specularity = _Specularity;
    s.specularTint = _SpecularTint;
    s.roughness = _Roughness;
    aTeamColor(s);
    aDecal(s);
    aWetness(s);
    aTwoSided(s);
    aRim(s);
    aEmission(s);
}

#endif // A_DEFINITIONS_SPEED_TREE_CGINC
