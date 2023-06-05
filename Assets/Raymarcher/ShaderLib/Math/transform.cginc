#ifndef TRANSFORM
#define TRANSFORM

float2 Rotate2D(float2 space, float angle)
{
    return mul(float2x2(cos(angle), sin(angle), -sin(angle), cos(angle)), space);
}

float2 RST2D(float2 p, float angle, float2 scale, float2 translation)
{
    float2 space = Rotate2D(p, angle);
    space *= scale;
    space += translation;
    return space;
}
#endif