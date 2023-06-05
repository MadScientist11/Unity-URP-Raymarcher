//Source - https://iquilezles.org/articles/distfunctions/
#ifndef sdfop


inline float opRound(float d, float rad)
{
    return d - rad;
}

inline float opOnion(in float sdf, in float thickness)
{
    return abs(sdf) - thickness;
}

inline float opUnion(float d1, float d2) { return min(d1, d2); }

inline float opSubtraction(float d1, float d2) { return max(-d1, d2); }

inline float opIntersection(float d1, float d2) { return max(d1, d2); }

inline float opSmoothUnion(float d1, float d2, float k)
{
    float h = clamp(0.5 + 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return lerp(d2, d1, h) - k * h * (1.0 - h);
}

inline float opSmoothSubtraction(float d1, float d2, float k)
{
    float h = clamp(0.5 - 0.5 * (d2 + d1) / k, 0.0, 1.0);
    return lerp(d2, -d1, h) + k * h * (1.0 - h);
}

inline float opSmoothIntersection(float d1, float d2, float k)
{
    float h = clamp(0.5 - 0.5 * (d2 - d1) / k, 0.0, 1.0);
    return lerp(d2, d1, h) + k * h * (1.0 - h);
}
#endif