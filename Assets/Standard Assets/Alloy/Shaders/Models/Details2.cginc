// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

///////////////////////////////////////////////////////////////////////////////
/// @file Details2.cginc
/// @brief Unity Details2 model inputs and outputs.
///////////////////////////////////////////////////////////////////////////////

#ifndef A_MODELS_DETAILS2_CGINC
#define A_MODELS_DETAILS2_CGINC

#define A_TEX_UV_OFF
#define A_TEX_SCROLL_OFF
#define A_TANGENT_ON
#define A_SHADOW_SURFACE_ON

#include "Assets/Alloy/Shaders/Framework/Model.cginc"

#include "TerrainEngine.cginc"

void aVertex(
    inout AVertex v)
{
    aDeGammaVertexColor(v);

    // Adapt vertex data so we can reuse wind code.
    appdata_full IN;

    UNITY_INITIALIZE_OUTPUT(appdata_full, IN);
    IN.vertex = v.vertex;
    IN.tangent = v.tangent;
    IN.color = v.color;

    WavingGrassBillboardVert(IN);
    v.vertex = IN.vertex;
    v.color = IN.color;
}

void aFinalColor(
    inout half4 color,
    ASurface s)
{
    UNITY_APPLY_FOG(s.fogCoord, color);
}

void aFinalGbuffer(
    inout AGbuffer gb,
    ASurface s)
{

}

#endif // A_MODELS_DETAILS2_CGINC
