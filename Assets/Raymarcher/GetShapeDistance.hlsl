
#include "Assets/Raymarcher/ShaderLib/sdf.cginc"
#ifndef GetShapeDistance

float GetShapeDistance(float3 p, SurfaceData surfaceData)
{

    
    if (surfaceData.surfaceType == -988356906)
    {
        return opSubtraction(sdSphere(p- float3(14.35, 2.52, 0.38), surfaceData.scale.x),sdBox(p - float3(14.62, 1.48, 0.99), surfaceData.scale));
    }


    if (surfaceData.surfaceType == 1)
    {
        
                return sdSphere(p- surfaceData.position, surfaceData.scale.x);
            
    }


    if (surfaceData.surfaceType == 0)
    {
        
                return sdBox(p - surfaceData.position, surfaceData.scale);
            
    }


    return MAX_DIST;
}

#endif
            