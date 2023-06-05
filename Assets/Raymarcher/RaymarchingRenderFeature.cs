using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ProfilingScope = UnityEngine.Rendering.ProfilingScope;

namespace Raymarcher
{
    public class RaymarchingRenderFeature : ScriptableRendererFeature
    {
        class RaymarcherPass : ScriptableRenderPass
        {
            private RTHandle _raymarchOutput;
            private RTHandle _cameraColor;
            private Material _raymarchingMaterial;

            private Raymarcher _raymarcher;

            public void Setup(RaymarchingData raymarchingData)
            {
                _raymarcher = new Raymarcher(raymarchingData);
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
                colorCopyDescriptor.enableRandomWrite = true;
                RenderingUtils.ReAllocateIfNeeded(ref _raymarchOutput, colorCopyDescriptor,
                    name: "_RaymarchOutput");

                colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
                colorCopyDescriptor.enableRandomWrite = false;
                RenderingUtils.ReAllocateIfNeeded(ref _cameraColor, colorCopyDescriptor, name: "_CameraColor");

                _raymarcher.Initialize(new Vector2(colorCopyDescriptor.width, colorCopyDescriptor.height),
                    _cameraColor.nameID,
                    _raymarchOutput.nameID);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.isPreviewCamera)
                    return;

                CommandBuffer cmd = CommandBufferPool.Get("Raymarcher");
                using (new ProfilingScope(cmd, new ProfilingSampler("Raymarching")))
                {
                    context.ExecuteCommandBuffer(cmd);
                    cmd.Clear();

                    if (_raymarchOutput != null)
                    {
                        RTHandle camTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
                        Blitter.BlitCameraTexture(cmd, camTarget, _cameraColor);

                        _raymarcher.Render(cmd);

                        Blitter.BlitCameraTexture(cmd, _raymarchOutput, camTarget);
                    }
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
            }

            public void CleanUp()
            {
                _raymarcher.CleanUp();
                _raymarchOutput?.Release();
            }
        }


        [SerializeField] private ComputeShader _raymarchingCS;
        private RaymarcherPass _pass;
        private RaymarchingData _raymarchingData;

        public override void Create()
        {
            _pass = new RaymarcherPass();
            _raymarchingData = new RaymarchingData();
            _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
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
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                _raymarchingData.RenderingCamera = Camera.main;
            
            }
            else
            {
                _raymarchingData.RenderingCamera = Camera.current;
            
            }
            
            _raymarchingData.RaymarchingCS = _raymarchingCS;
            _raymarchingData.MainLight = FindObjectOfType<Light>();

            _pass.Setup(_raymarchingData);
        }

        protected override void Dispose(bool disposing)
        {
            _pass.CleanUp();
        }
    }
}