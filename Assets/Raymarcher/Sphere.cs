namespace Raymarcher
{
    public class Sphere : Surface
    {
        public override string EquationSdf => $@"
                return sdSphere(p- surfaceData.position, surfaceData.scale.x);
            ";

        public override int SurfaceTypeId { get; set; } = (int)SurfaceType.Sphere;
    }
}