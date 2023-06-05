#ifndef sdf2d
inline float CircleSDF(float2 p, float size)
{
    return length(p) - size;
}

inline float Annular(float sdf, float thickness)
{
    return abs(sdf) - thickness;
}

inline float RingSDF(float2 p, float size, float thickness)
{
    return Annular(CircleSDF(p, size), thickness);
}

//Segment - https://www.youtube.com/watch?v=PMltMdi1Wzg
inline float SegmentSDF(in float2 p, in float2 a, in float2 b)
{
    float2 pa = p - a, ba = b - a;
    float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
    return  length(pa - ba * h);
}

float PolygonSDF(float2 p, float2 polygons[11], int length)
{
    float d = dot(p - polygons[0], p - polygons[0]);
    float s = 1.0;

    for (int i = 0; i < length; i++)
    {
        // distance
        int i1 = i;
        int i2 = fmod(i + 1, length);
        float2 e = polygons[i2] - polygons[i1];
        float2 v = float2(p - polygons[i1]);
        float2 pq = v - (e * clamp(dot(v, e) / dot(e, e), 0.0, 1.0));
        d = min(d, dot(pq, pq));

        // winding number from http://geomalgorithms.com/a03-_inclusion.html
        // with a bit of help from https://www.shadertoy.com/view/wdBXRW
        float3 cond = float3(p.y >= polygons[i1].y, p.y < polygons[i2].y, e.x* v.y > e.y * v.x);
        if (all(cond) || all(!(cond))) s *= -1.0;  // have a valid up or down intersect 
    }

    return sqrt(d) * s;
}

// Source - https://forum.unity.com/threads/antialiasing-circle-shader.432119/#post-2795633
inline float SampleHard(float sdf, float offset = 1.5)
{
    float pwidth = length(float2(ddx(sdf), ddy(sdf)));
    return smoothstep(0, -pwidth * offset, sdf);
}

inline float SampleLines(float sdf, float factor = 10)
{
    return sin(sdf * factor);
}
#endif

