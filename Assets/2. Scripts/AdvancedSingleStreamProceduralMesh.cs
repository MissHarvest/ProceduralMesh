using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedSingleStreamProceduralMesh : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public Vector3 position, normal;
        public Vector4 tangent;
        public Vector2 texCoord0;
    }

    private void OnEnable()
    {
        int vertexAttributesCount = 4;
        int vertexCount = 4;

        var mesh = new Mesh
        {
            name = "ASS Procedural Mesh"
        };

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            vertexAttributesCount, Allocator.Temp);

        vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension: 3);
        vertexAttributes[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4);
        vertexAttributes[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension: 2);

        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<Vertex> vertices = meshData.GetVertexData<Vertex>();

        var vertex = new Vertex
        {
            normal = Vector3.back,
            tangent = new Vector4(1, 0, 0, -1)
        };

        vertex.position = Vector3.zero;
        vertex.texCoord0 = Vector2.zero;
        vertices[0] = vertex;

        vertex.position = Vector3.right;
        vertex.texCoord0 = Vector2.right;
        vertices[1] = vertex;

        vertex.position = Vector3.up;
        vertex.texCoord0 = Vector2.up;
        vertices[2] = vertex;

        vertex.position = new Vector3(1, 1, 0);
        vertex.texCoord0 = Vector2.one;
        vertices[3] = vertex;

        meshData.SetIndexBufferParams(6, IndexFormat.UInt16);
        NativeArray<ushort> trianglesIndices = meshData.GetIndexData<ushort>();
        trianglesIndices[0] = 0;
        trianglesIndices[1] = 2;
        trianglesIndices[2] = 1;
        trianglesIndices[3] = 1;
        trianglesIndices[4] = 2;
        trianglesIndices[5] = 3;

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, 6));

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
