using System.Collections;
using System.Collections.Generic;
using Raymarcher;
using UnityEngine;

public class Torus : Surface
{
    public override string EquationSdf => $@"
                return sdTorus(p-surfaceData.position, surfaceData.scale);
            ";

    public override int SurfaceTypeId { get; set; } = (int)SurfaceType.Torus;
}
