using ProceduralMeshes;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;
using System.Xml.XPath;
using UnityEditor.Rendering;

public struct PointyHexaGrid : IMeshGenerator
{
    public int VertexCount => 7 * Resolution * Resolution;

    public int IndexCount => 18 * Resolution * Resolution;

    public int JobLength => 1 * Resolution;

    public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

    public int Resolution { get; set; }

    public void Excutes<S>(int i, S stream) where S : struct, IMeshStreams
    {
        int vi = 7 * i * Resolution;
        int ti = 6 * i * Resolution;

        var vertex = new Vertex();
        vertex.normal.z = -1.0f;
        vertex.tangent.xw = float2(1.0f, -1.0f);

        var h = sqrt(3) / 4f;
        var xCoordinate = float2(-h, h) / Resolution;
        var yCoordinate = float4(-0.5f, -0.25f, 0.25f, 0.5f) / Resolution;
        
        for(int x = 0; x < Resolution; ++x, vi += 7, ti += 6)
        {
            var centorOffset = float2((Resolution - 1) * h, (Resolution - 1) * 0.375f);            
            
            var xOffset = i % 2 == 0 ? 0 : h;
            var centor = (float2(2 * h * x + xOffset, 0.75f * i) - centorOffset) / Resolution;

            // 0
            vertex.position.x = centor.x;
            vertex.position.y = centor.y;
            vertex.uv = 0.5f;
            stream.SetVertex(vi + 0, vertex);            

            // 1
            vertex.position.y = centor.y + yCoordinate.x;
            vertex.uv = float2(0.5f, 0);
            stream.SetVertex(vi + 1, vertex);

            // 2
            vertex.position.x = centor.x + xCoordinate.x;
            vertex.position.y = centor.y + yCoordinate.y;
            vertex.uv = float2(0.5f - h, 0.25f);
            stream.SetVertex(vi + 2, vertex);

            // 3
            vertex.position.y = centor.y + yCoordinate.z;
            vertex.uv = float2(0.5f - h, 0.75f);
            stream.SetVertex(vi + 3, vertex);

            // 4
            vertex.position.x = centor.x;
            vertex.position.y = centor.y + yCoordinate.w;
            vertex.uv = float2(0.5f, 1.0f);
            stream.SetVertex(vi + 4, vertex);

            // 5
            vertex.position.x = centor.x + xCoordinate.y;
            vertex.position.y = centor.y + yCoordinate.z;
            vertex.uv = float2(0.5f + h, 0.75f);
            stream.SetVertex(vi + 5, vertex);

            // 6
            vertex.position.y = centor.y + yCoordinate.y;
            vertex.uv = float2(0.5f + h, 0.25f);
            stream.SetVertex(vi + 6, vertex);

            stream.SetTriangle(ti + 0, vi + int3(0, 1, 2));
            stream.SetTriangle(ti + 1, vi + int3(0, 2, 3));
            stream.SetTriangle(ti + 2, vi + int3(0, 3, 4));
            stream.SetTriangle(ti + 3, vi + int3(0, 4, 5));
            stream.SetTriangle(ti + 4, vi + int3(0, 5, 6));
            stream.SetTriangle(ti + 5, vi + int3(0, 6, 1));
        }
    }
}
