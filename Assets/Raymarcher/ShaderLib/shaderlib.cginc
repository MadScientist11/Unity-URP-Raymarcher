#ifndef shaderlib

#define tt  _Time.y

//Works only in fragment shader, because GPU renders fragments 2x2
float2 AspectRatioUV(float2 uv)
{
    float aspect = ddy(uv.y) / ddx(uv.x);
    uv.x *= aspect;
    return uv;
}
#endif
