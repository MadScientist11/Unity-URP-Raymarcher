using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaymarcherSettings", menuName = "Raymarcher/RaymarcherSettings")]
public class RaymarcherSettings : ScriptableObject
{
    public ComputeShader RaymarchingCS;
    public int KernelIndex => RaymarchingCS.FindKernel("Raymarch");
    public int SourceId => Shader.PropertyToID("Source");
    public int DestinationId => Shader.PropertyToID("Destination");
  
    public float MaxDepth;
}
