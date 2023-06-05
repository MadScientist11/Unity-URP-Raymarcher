//Source - https://iquilezles.org/articles/distfunctions/
#ifndef sdf3d
inline float sdSphere( float3 p, float s )
{
    return length(p)-s;
}

inline float sdBox( float3 p, float3 b )
{
    float3 q = abs(p) - b;
    return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);
}


inline float sdRoundBox( float3 p, float3 b, float r )
{
    float3 q = abs(p) - b;
    return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0) - r;
}

inline float sdTorus( float3 p, float3 t )
{
    float2 q = float2(length(p.xz)-t.x,p.y);
    return length(q)-t.y;
}

inline float sdPlane( float3 p, float3 n, float h )
{
    // n must be normalized
    return dot(p,n) + h;
}

#endif
