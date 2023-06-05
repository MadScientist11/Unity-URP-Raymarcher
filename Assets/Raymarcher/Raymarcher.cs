using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Raymarcher
{
    
    public struct SurfaceData
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public int surfaceType;

        public static int GetSize()
        {
            return sizeof(float) * 9 + sizeof(int) * 1;
        }
    }
    public class Raymarcher
    {
        private readonly RaymarchingData _raymarchingData;
        private readonly ComputeShader _raymarchingCS;
        private readonly int _raymarchKernel;
        private readonly int _sourceId;
        private readonly int _destId;
        private Vector2 _dimensions;
        private RenderTargetIdentifier _source;
        private RenderTargetIdentifier _destination;

        private readonly List<ComputeBuffer> _buffersToDispose = new();

        public Raymarcher(RaymarchingData raymarchingData)
        {
            _raymarchingData = raymarchingData;
            _raymarchingCS = raymarchingData.RaymarchingCS;
            _raymarchKernel = _raymarchingCS.FindKernel("CSMain");
            _sourceId = Shader.PropertyToID("Source");
            _destId = Shader.PropertyToID("Destination");
        }

        public void Initialize(Vector2 dimensions, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            _destination = destination;
            _source = source;
            _dimensions = dimensions;
        }

        public void Render(CommandBuffer cmd)
        {
            SetUpRenderer(cmd);
            
            _raymarchingCS.GetKernelThreadGroupSizes(_raymarchKernel, out uint x, out uint y, out _);
            int threadGroupsX = Mathf.CeilToInt(_dimensions.x / (float)x);
            int threadGroupsY = Mathf.CeilToInt(_dimensions.y / (float)y);
            _raymarchingCS.Dispatch(_raymarchKernel, threadGroupsX, threadGroupsY, 1);
            
            CleanUp();

        }

        private void SetUpRenderer(CommandBuffer cmd)
        {
            cmd.SetComputeTextureParam(_raymarchingCS, _raymarchKernel, _sourceId, _source);
            cmd.SetComputeTextureParam(_raymarchingCS, _raymarchKernel, _destId, _destination);
            cmd.SetComputeMatrixParam(_raymarchingCS, "_CameraToWorld",
                _raymarchingData.RenderingCamera.cameraToWorldMatrix);
            cmd.SetComputeMatrixParam(_raymarchingCS, "_CameraInverseProjection",
                _raymarchingData.RenderingCamera.projectionMatrix.inverse);
            cmd.SetComputeVectorParam(_raymarchingCS, "_CameraForward", _raymarchingData.RenderingCamera.transform.forward);
            cmd.SetComputeVectorParam(_raymarchingCS, "_LightDirection", _raymarchingData.MainLight.transform.forward);
            
            

            SetSurfaceData(cmd);

        }

        private void SetSurfaceData(CommandBuffer cmd)
        {
            Surface[] surfaces = Object.FindObjectsOfType<Surface>().Where(x => x.RenderSurface).ToArray();
            cmd.SetComputeIntParam(_raymarchingCS, "surfaceCount", surfaces.Length);
            SurfaceData[] surfaceData = new SurfaceData[surfaces.Length];
            

            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaceData[i] = new SurfaceData()
                {
                    position = surfaces[i].Position,
                    scale = surfaces[i].Scale,
                    rotation = surfaces[i].Rotation,
                    surfaceType = surfaces[i].SurfaceTypeId,
                };
            }
            
            ComputeBuffer surfacesBuffer = null;
            if (surfaces.Length != 0)
            {
                surfacesBuffer = new ComputeBuffer(surfaceData.Length, SurfaceData.GetSize());
                surfacesBuffer.SetData(surfaceData);
                _raymarchingCS.SetBuffer(_raymarchKernel, "surfaceData", surfacesBuffer);
            }

            _buffersToDispose.Add(surfacesBuffer);
        }
        public void CleanUp()
        {
            foreach (var buffer in _buffersToDispose)
            {
                buffer?.Dispose();
            }
        }
    }
}