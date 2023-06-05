using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Raymarcher
{
    public class CustomSurface : Surface
    {
        public override string EquationSdf => SdfEquation;
        public override int SurfaceTypeId { get; set; } = (int)SurfaceType.Custom;


        [Multiline] public string SdfEquation;


        [ContextMenu(nameof(UpdateEquation))]
        private void UpdateEquation()
        {
            OnReset();
            RaymarcherSDFsInjector.UpdateSDFs();
        }

        protected override void OnReset()
        {
          
                SurfaceTypeId = HelperFunctions.Hash(SdfEquation);
        }
    }
}