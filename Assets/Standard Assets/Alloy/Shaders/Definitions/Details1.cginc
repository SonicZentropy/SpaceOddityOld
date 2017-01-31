// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Details1.cginc
/// @brief Unity Terrain details WavingDoublePass surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_DETAILS1_CGINC
#define A_DEFINITIONS_DETAILS1_CGINC

#include "Assets/Alloy/Shaders/Models/Details1.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{
    half4 base = aSampleBase(s) * s.vertexColor;

    s.baseColor = base.rgb;
    s.opacity = base.a;
    aCutout(s);
    s.specularity = 0.5h;
    s.roughness = 1.0h;
    s.opacity *= s.vertexColor.a;
}

#endif // A_DEFINITIONS_DETAILS1_CGINC
