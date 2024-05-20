using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct SqureGrid : IMeshGenerator
    {
        public int VertexCount => 4;

        public int IndexCount => 6;

        public int JobLength => 1;

        public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

        public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
        {
            var vertex = new Vertex();
            vertex.normal.z = -1f;
            vertex.tangent.xw = float2(1f, -1f);
            stream.SetVertex(0, vertex);

            vertex.position = right();
            vertex.uv = float2(1f, 0f);
            stream.SetVertex(1, vertex);

            vertex.position = up();
            vertex.uv = float2(0f, 1f);
            stream.SetVertex(2, vertex);

            vertex.position = float3(1f, 1f, 0f);
            vertex.uv = 1f;
            stream.SetVertex(3, vertex);

            stream.SetTriangle(0, int3(0, 2, 1));
            stream.SetTriangle(1, int3(1, 2, 3));
        }
    }
}
