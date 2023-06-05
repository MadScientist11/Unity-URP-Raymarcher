#ifndef NOISE
#define NOISE
#include "hash.cginc"

// https://www.youtube.com/watch?v=zXsWftRdsvU&t=826s&ab_channel=TheArtofCode
float ValueNoise(float2 uv)
{
    float2 id = floor(uv);
    float2 t = frac(uv);
    t = t * t * (3 - 2 * t);

    float bottomLeft = hash21(id);
    float bottomRight = hash21(id + float2(1, 0));
    float bottomValue = lerp(bottomLeft, bottomRight, t.x);

    float upperLeft = hash21(id + float2(0, 1));
    float upperRight = hash21(id + float2(1, 1));
    float upperValue = lerp(upperLeft, upperRight, t.x);

    float noise = lerp(bottomValue, upperValue, t.y);

    return noise;
}

float ValueNoiseFBM(float2 uv, float octaves = 6, float gain = 0.5, float lacunarity = 2)
{
    float value = 0.0;
    float amplitude = .5;
    float frequency = 1.;
    float range = frequency;

    for (int i = 0; i < octaves; i++)
    {
        value += amplitude * ValueNoise(uv * frequency);
        frequency *= lacunarity;
        amplitude *= gain;
        range += amplitude;
    }

    return value / range;
}

#endif