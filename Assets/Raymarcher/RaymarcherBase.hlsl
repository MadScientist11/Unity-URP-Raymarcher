#ifndef RAYMARCHER_BASE

struct Surface
{
   
    float distanceToSurface;
};
struct SurfaceData
{
    float3 position;
    float3 rotation;
    float3 scale;
    int surfaceType;
};


#define SURF_DIST 0.001
#define MAX_STEPS 150
#define MAX_DIST 100.0


struct Ray
{
    float3 origin;
    float3 direction;
};

Ray CreateRay(float3 origin, float3 direction)
{
    Ray ray;
    ray.origin = origin;
    ray.direction = direction;
    return ray;
}

Ray CreateCameraRay(float2 uv, float4x4 cameraToWorldMatrix, float4x4 CameraInverseProjectionMatrix)
{
    float3 origin = mul(cameraToWorldMatrix, float4(0, 0, 0, 1)).xyz;
    float3 direction = mul(CameraInverseProjectionMatrix, float4(uv, 0, 1)).xyz;
    direction = mul(cameraToWorldMatrix, float4(direction, 0)).xyz;
    direction = normalize(direction);
    return CreateRay(origin, direction);
}


#endif
