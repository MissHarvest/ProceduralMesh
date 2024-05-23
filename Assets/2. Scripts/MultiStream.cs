using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralMeshes.Streams
{
    public struct MultiStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]
        NativeArray<float3> positions;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<float3> normals;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<float4> tangents;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<float2> uvs;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleUint16> triangles;

        public void Setup(Mesh.MeshData data, Bounds bounds, int vertexCount, int indexCount)
        {
            var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            vertexAttributes[0] = new VertexAttributeDescriptor(
                VertexAttribute.Position, dimension: 3, stream: 0);
            vertexAttributes[1] = new VertexAttributeDescriptor(
                VertexAttribute.Normal, dimension:3, stream: 1);
            vertexAttributes[2] = new VertexAttributeDescriptor(
                VertexAttribute.Tangent, dimension:4, stream: 2);
            vertexAttributes[3] = new VertexAttributeDescriptor(
                VertexAttribute.TexCoord0, dimension:2, stream: 3);

            data.SetVertexBufferParams(vertexCount, vertexAttributes);
            vertexAttributes.Dispose();

            data.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

            data.subMeshCount = 1;
            data.SetSubMesh(0, new SubMeshDescriptor(0, indexCount),
                MeshUpdateFlags.DontRecalculateBounds |
                MeshUpdateFlags.DontValidateIndices);

            positions = data.GetVertexData<float3>(0);
            normals = data.GetVertexData<float3>(1);
            tangents = data.GetVertexData<float4>(2);
            uvs = data.GetVertexData<float2>(3);

            triangles = data.GetIndexData<TriangleUint16>();
        }

        public void SetTriangle(int index, int3 triangle)
        {
            triangles[index] = triangle;
        }

        public void SetVertex(int index, Vertex data)
        {
            positions[index] = data.position;
            normals[index] = data.normal;
            tangents[index] = data.tangent;
            uvs[index] = data.uv;
        }
    }
}