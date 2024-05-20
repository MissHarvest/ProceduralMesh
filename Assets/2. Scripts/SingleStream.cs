using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralMeshes.Streams
{
    public struct SingleStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Stream0
        {
            public float3 position, normal;
            public float4 tangent;
            public float2 texCoord0;
        }

        [NativeDisableContainerSafetyRestriction]
        NativeArray<Stream0> stream0;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleUint16> triangles;

        public void Setup(Mesh.MeshData data, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(
                4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            descriptor[0] = new VertexAttributeDescriptor(
                VertexAttribute.Position, dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(
                VertexAttribute.Normal, dimension: 3);
            descriptor[2] = new VertexAttributeDescriptor(
                VertexAttribute.Tangent, dimension: 4);
            descriptor[3] = new VertexAttributeDescriptor(
                VertexAttribute.TexCoord0, dimension: 2);

            data.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            data.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
            
            data.subMeshCount = 1;
            data.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
            {
                bounds = bounds,
                vertexCount = vertexCount
            },
            MeshUpdateFlags.DontRecalculateBounds|
            MeshUpdateFlags.DontValidateIndices);

            stream0 = data.GetVertexData<Stream0>(); // 왜 Vertex로 안하고 Stream 으로..
            //triangles = data.GetIndexData<ushort>().Reinterpret<TriangleUint16>(4); // 이 코드가 의미하는 것
            triangles = data.GetIndexData<TriangleUint16>();
        }
        // Vertex 에 sequential layout 을 적용했다면 가능은 하다.
        // 하지만 이렇게 함으로써, 유연성을 줄 수 있다.
        // 예를 들어 Vertx를 조정하지 않고도 색상과 같은 데이터를 추가할 수 있다.

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex data) => stream0[index] = new Stream0
        {
            position = data.position,
            normal = data.normal,
            tangent = data.tangent,
            texCoord0 = data.uv
        };

        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;
    }
}
