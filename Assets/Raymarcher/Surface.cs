using System;
using UnityEngine;

namespace Raymarcher
{
    public abstract class Surface : MonoBehaviour
    {

        public Vector3 Position => transform.position;
        public Vector3 Rotation =>
            new(transform.rotation.eulerAngles.x * Mathf.Deg2Rad,
                transform.rotation.eulerAngles.y * Mathf.Deg2Rad, transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

        public Vector3 Scale => transform.lossyScale;
        
        public abstract string EquationSdf { get; }

        public abstract int SurfaceTypeId { get; set; }
        public bool RenderSurface { get; set; } = true;
        

        private void Reset()
        {
            OnReset();
            RaymarcherSDFsInjector.UpdateSDFs();
        }

       
        protected virtual void OnReset()
        {
            
        } 
    }
}
