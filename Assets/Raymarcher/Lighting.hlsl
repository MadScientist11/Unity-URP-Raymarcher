#ifndef RAYMARCHER_LIGHTING
    float LambertLighting(float3 normalDir, float3 lightDir)
    {
        return max(dot(normalDir, lightDir), 0.);
    }
#endif
