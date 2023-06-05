using System.Collections;
using System.Collections.Generic;
using Raymarcher.PostProcess;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPass : ScriptableRenderPass
{
    private OutlineSettings _settings;
    private Material _outlineMaterial;
    
    private RTHandle _copiedColor;
    private RTHandle _output;
    public void Setup(OutlineSettings settings, Material outlineMaterial)
    {
        _settings = settings;
        _outlineMaterial = outlineMaterial;
     
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if(_outlineMaterial == null)
            return;
     
        
        RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
        colorCopyDescriptor.enableRandomWrite = true;
        RenderingUtils.ReAllocateIfNeeded(ref _output, colorCopyDescriptor,
            name: "OutlineOutput");

        colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
        colorCopyDescriptor.enableRandomWrite = false;
        RenderingUtils.ReAllocateIfNeeded(ref _copiedColor, colorCopyDescriptor, name: "_CameraColor");
    }


    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isPreviewCamera || !_settings.IsActive() || _outlineMaterial == null)
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("OutlinePostPrcoess");
        using (new ProfilingScope(cmd, new ProfilingSampler("OutlinePostPrcoess")))
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            if (_output != null)
            {
                RTHandle camTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
                Blitter.BlitCameraTexture(cmd, camTarget, _copiedColor);


                _outlineMaterial.SetTexture("_BlitTexture", _copiedColor);
                _outlineMaterial.SetFloat("_Thickness", _settings.Thickness.value);
                Blitter.BlitCameraTexture(cmd, _copiedColor, _output, _outlineMaterial, 0);
                Blitter.BlitCameraTexture(cmd, _output, camTarget, Vector2.one);
            }
            

       
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }
    
    public void CleanUp()
    {
        _output?.Release();
    }
}
