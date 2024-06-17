using ProceduralMeshes;
using System.Collections;
using System.Collections.Generic;
using static Unity.Mathematics.math;
using UnityEngine;

public struct MySharedTriangleGrid : IMeshGenerator
{
    public int VertexCount => (Resolution + 1) * (Resolution + 1);

    public int IndexCount => Resolution * Resolution * 6;

    public int JobLength => Resolution + 1;

    public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

    public int Resolution { get; set; }

    public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
    {
        int vi = (Resolution + 1) * i;
        int ti = 2 * Resolution * (i - 1);

        var xStart = i % 2 == 0 ? 0 : 0.5f;
        var h = sqrt(3) / 2;
        
        var xOffset = -(Resolution + 0.5f) / 2;
        var yOffset = -(h * Resolution) / 2;

        var vertex = new Vertex();
        vertex.normal.z = -1.0f;
        vertex.tangent.xw = float2(1.0f, -1.0f);
        
        vertex.position.x = (xStart + xOffset) / Resolution;
        vertex.position.y = (h * i + yOffset) / Resolution;
        vertex.uv.x = xStart / Resolution;
        vertex.uv.y = (h * i) / Resolution;
        stream.SetVertex(vi, vertex);
        ++vi;

        for (int x = 1; x <= Resolution; ++x, ++vi, ti += 2)
        {
            vertex.position.x = (x + xStart + xOffset) / Resolution;
            vertex.uv.x = (x + xStart) / Resolution;
            stream.SetVertex(vi, vertex);

            if(i > 0)
            {
                int c1 = i % 2 == 0 ? 0 : -Resolution - 1;
                int c2 = i % 2 == 0 ? -Resolution - 2 : -1;
                stream.SetTriangle(ti + 0, vi + int3(-Resolution-2, -1, c1));
                stream.SetTriangle(ti + 1, vi + int3(-Resolution - 1, c2, 0));
            }
        }
    }
}
