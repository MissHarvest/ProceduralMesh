using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct UVSphere : IMeshGenerator
    {
        public int VertexCount => (Resolution + 1) * (Resolution + 1);

        public int IndexCount => 6 * Resolution * Resolution;

        public int JobLength => Resolution + 1;

        public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

        public int Resolution { get; set; }

        public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
        {
            int vi = (Resolution + 1) * i;
            int ti = 2 * Resolution * (i - 1);

            var vertex = new Vertex();
            vertex.normal.z = -1f;
            vertex.tangent.xw = float2(1f, -1f);

            vertex.position.x = (float)i / Resolution - 0.5f;
            vertex.position.y = Resolution / Resolution - 0.5f;
            vertex.uv.x = (float)i / Resolution;
            vertex.uv.y = 1.0f;
            stream.SetVertex(vi, vertex);

            vi += 1;
            for(int y = 1; y <= Resolution; ++y, ++vi, ti += 2)
            {
                vertex.position.y = (float)(Resolution - y) / Resolution - 0.5f;
                vertex.uv.y = (float)(Resolution - y) / Resolution;
                stream.SetVertex(vi, vertex);

                if(i > 0)
                {
                    stream.SetTriangle(ti + 0, vi + int3(-Resolution - 2, -1, -Resolution - 1));
                    stream.SetTriangle(ti + 1, vi + int3(-Resolution - 1, -1, 0));
                }
            }
        }
    }
}
