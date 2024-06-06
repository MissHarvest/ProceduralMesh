using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct SharedTriangleGrid : IMeshGenerator
    {
        public int VertexCount => (Resolution + 1) * (Resolution + 1);

        public int IndexCount => 6 * Resolution * Resolution;

        public int JobLength => 1 + Resolution;

        public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

        public int Resolution { get; set; }

        public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
        {
            int vi = (Resolution + 1) * i;
            int ti = (i - 1) * 2 * Resolution;

            var vertex = new Vertex();
            vertex.normal.z = -1.0f;
            vertex.tangent.xw = float2(1.0f, -1.0f);

            var xOffset = (i % 2 == 0 ? -0.25f : 0.25f) / Resolution - 0.5f;
            var uOffset = (i % 2 == 0 ? 0 : 0.5f / (Resolution + 0.5f));

            vertex.position.x = xOffset;            
            vertex.position.y = ((float)i / Resolution - 0.5f) * sqrt(3) / 2f;

            vertex.uv.x = (vertex.position.x / (1 + 0.5f / Resolution)) + 0.5f;
            vertex.uv.y = (vertex.position.y / (1 + 0.5f / Resolution)) + 0.5f;
            stream.SetVertex(vi, vertex);

            ++vi;
            for(var x = 1; x <= Resolution; ++x, ++vi, ti += 2)
            {
                vertex.position.x = (float)x / Resolution + xOffset;
                vertex.uv.x = (vertex.position.x / (1 + 0.5f / Resolution)) + 0.5f;
                stream.SetVertex(vi, vertex);

                if(i > 0)
                {
                    int cy = i % 2 == 0 ? -Resolution - 1 : 0;
                    int dy = i % 2 == 0 ? -1 : -Resolution - 2;
                    stream.SetTriangle(ti + 0, vi + int3(-Resolution - 2, -1, cy));
                    stream.SetTriangle(ti + 1, vi + int3(-Resolution - 1, dy, 0));
                }
            }
        }
    }
}
