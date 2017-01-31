// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

/////////////////////////////////////////////////////////////////////////////////
/// @file Details0.cginc
/// @brief Unity Terrain details VertexLit surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef A_DEFINITIONS_DETAILS0_CGINC
#define A_DEFINITIONS_DETAILS0_CGINC

#include "Assets/Alloy/Shaders/Models/Standard.cginc"
#include "Assets/Alloy/Shaders/Lighting/Standard.cginc"

void aSurface(
    inout ASurface s)
{
    half4 base = aSampleBase(s) * s.vertexColor;

    s.baseColor = base.rgb;
    s.opacity = base.a;
    s.specularity = 0.5h;
    s.roughness = 1.0h;
}

#endif // A_DEFINITIONS_DETAILS0_CGINC
