using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct SqureGrid : IMeshGenerator
    {
        public int VertexCount => 4 * Resolution * Resolution;

        public int IndexCount => 6 * Resolution * Resolution;

        public int JobLength => 1 * Resolution;

        public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

        public int Resolution { get; set; }

        public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
        {
            int vi = 4 * i * Resolution;
            int ti = 2 * i * Resolution;

            int y = i;
            
            for (int x = 0; x < Resolution; ++x, vi +=4, ti += 2)
            {
                var xCoordinates = float2(x, x + 1) / Resolution - 0.5f;
                var yCoordinates = float2(y, y + 1) / Resolution - 0.5f;

                var vertex = new Vertex();
                vertex.position.x = xCoordinates.x;
                vertex.position.y = yCoordinates.x;
                vertex.normal.z = -1f;
                vertex.tangent.xw = float2(1f, -1f);
                stream.SetVertex(vi + 0, vertex);

                vertex.position.x = xCoordinates.y;
                vertex.uv = float2(1f, 0f);
                stream.SetVertex(vi + 1, vertex);

                vertex.position.x = xCoordinates.x;
                vertex.position.y = yCoordinates.y;
                vertex.uv = float2(0f, 1f);
                stream.SetVertex(vi + 2, vertex);

                vertex.position.x = xCoordinates.y;
                vertex.uv = 1f;
                stream.SetVertex(vi + 3, vertex);

                stream.SetTriangle(ti + 0, vi + int3(0, 2, 1));
                stream.SetTriangle(ti + 1, vi + int3(1, 2, 3));
            }
        }
    }
}
