// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

///////////////////////////////////////////////////////////////////////////////
/// @file Details1.cginc
/// @brief Unity Details1 model inputs and outputs.
///////////////////////////////////////////////////////////////////////////////

#ifndef A_MODELS_DETAILS1_CGINC
#define A_MODELS_DETAILS1_CGINC

#define A_TEX_UV_OFF
#define A_TEX_SCROLL_OFF
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
    IN.color = v.color;

    WavingGrassVert(IN);
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

#endif // A_MODELS_DETAILS1_CGINC
