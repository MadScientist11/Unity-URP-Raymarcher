#ifndef LERP
#define LERP


inline float LerpUnclamped(float a, float b, float t)
{
    return ( 1 -t ) * a + b * t;
}

inline float2 LerpUnclamped(float2 a, float2 b, float t)
{
    return float2(LerpUnclamped(a.x, b.x, t), LerpUnclamped(a.y, b.y, t)); 
}

inline float3 LerpUnclamped(float3 a, float3 b, float t)
{
    return float3(LerpUnclamped(a.x, b.x, t), LerpUnclamped(a.y, b.y, t),  LerpUnclamped(a.z, b.z, t)); 
}

inline float4 LerpUnclamped(float4 a, float4 b, float t)
{
    return float4(LerpUnclamped(a.x, b.x, t), LerpUnclamped(a.y, b.y, t),  LerpUnclamped(a.z, b.z, t), LerpUnclamped(a.w, b.w, t)); 
}

inline float Lerp(float a, float b, float t)
{
    return LerpUnclamped(a, b, saturate(t)); 
}

inline float2 Lerp(float2 a, float2 b, float t)
{
    return float2(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t)); 
}

inline float3 Lerp(float3 a, float3 b, float t)
{
    return float3(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t),  Lerp(a.z, b.z, t)); 
}

inline float4 Lerp(float4 a, float4 b, float t)
{
    return float4(Lerp(a.x, b.x, t), Lerp(a.y, b.y, t),  Lerp(a.z, b.z, t), Lerp(a.w, b.w, t)); 
}

#endif