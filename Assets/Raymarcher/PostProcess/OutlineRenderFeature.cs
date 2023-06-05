using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Raymarcher.PostProcess
{
    public class OutlineRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Material _outlineMaterial;
        private OutlineRenderPass _pass;

        private OutlineSettings _outlineSettings;

        public override void Create()
        {
            _pass = new OutlineRenderPass();
            _pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _outlineSettings = VolumeManager.instance.stack.GetComponent<OutlineSettings>();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            CameraType cameraType = renderingData.cameraData.cameraType;
            if (cameraType == CameraType.Preview) return;

            _pass.ConfigureInput(ScriptableRenderPassInput.Color);
            renderer.EnqueuePass(_pass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            _pass.Setup(_outlineSettings, _outlineMaterial);
        }

        protected override void Dispose(bool disposing)
        {
            _pass.CleanUp();
        }
    }
}
