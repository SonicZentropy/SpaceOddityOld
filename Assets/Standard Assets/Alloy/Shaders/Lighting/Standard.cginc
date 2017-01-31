// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

///////////////////////////////////////////////////////////////////////////////
/// @file Standard.cginc
/// @brief Physical BRDF with optional SSS effects. Forward+Deferred.
///////////////////////////////////////////////////////////////////////////////

#ifndef A_LIGHTING_STANDARD_CGINC
#define A_LIGHTING_STANDARD_CGINC

#include "Assets/Alloy/Shaders/Framework/Lighting.cginc"

void aPreLighting(
    inout ASurface s)
{
    aStandardPreLighting(s);
}

half3 aDirect( 
    ADirect d,
    ASurface s)
{
    return aStandardDirect(d, s);
}

half3 aIndirect(
    AIndirect i,
    ASurface s)
{
    return aStandardIndirect(i, s);
}

#endif // A_LIGHTING_STANDARD_CGINC
