namespace Raymarcher
{
    public class Box : Surface
    {
        public override string EquationSdf => $@"
                return sdBox(p - surfaceData.position, surfaceData.scale);
            ";

        public override int SurfaceTypeId { get; set; } = (int)SurfaceType.Cube;
    }
}