#ifndef INV_LERP
#define INV_LERP
inline float InverseLerpUnclamped(float a, float b, float t)
{
    return (t - a) / (b - a);
}

inline float2 InverseLerpUnclamped(float2 a, float2 b, float t)
{
    return float2(InverseLerpUnclamped(a.x, b.x, t), InverseLerpUnclamped(a.y, b.y, t)); 
}

inline float3 InverseLerpUnclamped(float3 a, float3 b, float t)
{
    return float3(InverseLerpUnclamped(a.x, b.x, t), InverseLerpUnclamped(a.y, b.y, t),  InverseLerpUnclamped(a.z, b.z, t)); 
}

inline float4 InverseLerpUnclamped(float4 a, float4 b, float t)
{
    return float4(InverseLerpUnclamped(a.x, b.x, t), InverseLerpUnclamped(a.y, b.y, t),  InverseLerpUnclamped(a.z, b.z, t), InverseLerpUnclamped(a.w, b.w, t)); 
}

inline float InverseLerp(float a, float b, float t)
{
    return saturate(InverseLerpUnclamped(a, b, t));
}

inline float2 InverseLerp(float2 a, float2 b, float t)
{
    return float2(InverseLerp(a.x, b.x, t), InverseLerp(a.y, b.y, t)); 
}

inline float3 InverseLerp(float3 a, float3 b, float t)
{
    return float3(InverseLerp(a.x, b.x, t), InverseLerp(a.y, b.y, t),  InverseLerp(a.z, b.z, t)); 
}

inline float4 InverseLerp(float4 a, float4 b, float t)
{
    return float4(InverseLerp(a.x, b.x, t), InverseLerp(a.y, b.y, t),  InverseLerp(a.z, b.z, t), InverseLerp(a.w, b.w, t)); 
}
#endif