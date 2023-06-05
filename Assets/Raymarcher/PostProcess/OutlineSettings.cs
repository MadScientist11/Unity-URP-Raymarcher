using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Raymarcher.PostProcess
{
    [System.Serializable, VolumeComponentMenu("Raymarcher/Outline")]
    public class OutlineSettings : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter Thickness = new ClampedFloatParameter(0f, 0f, 0.01f);
        public bool IsActive()
        {
            return active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}
