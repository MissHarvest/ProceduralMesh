using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedMultiStreamProceduralMesh : MonoBehaviour
{
    private void OnEnable()
    {
        int vertexAttributeCount = 4;
        int vertexCount = 4;
        int trianglesIndexCount = 6;

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1); // 생성하려는 메쉬의 수
        // 메쉬의 수는 무엇을 기준으로 나누는가
        Mesh.MeshData meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );

        var mesh = new Mesh
        {
            name = "AMS Procedural Mesh",
        };

        vertexAttributes[0] = new VertexAttributeDescriptor(
            dimension:3, stream: 0);
        vertexAttributes[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension:3, stream: 1);
        vertexAttributes[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4, stream: 2);
        vertexAttributes[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension:2, stream: 3);

        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<Vector3> positions = meshData.GetVertexData<Vector3>();
        positions[0] = Vector3.zero;
        positions[1] = Vector3.right;
        positions[2] = Vector3.up;
        positions[3] = new Vector3(1, 1, 0);

        NativeArray<Vector3> normals = meshData.GetVertexData<Vector3>(1);
        normals[0] = normals[1] = normals[2] = normals[3] = Vector3.back;

        NativeArray<Vector4> tangents = meshData.GetVertexData<Vector4>(2);
        tangents[0] = tangents[1] = tangents[2] = tangents[3] = new Vector4(1, 0, 0, -1);

        NativeArray<Vector2> uv = meshData.GetVertexData<Vector2>(3);
        uv[0] = Vector2.zero;
        uv[1] = Vector2.right;
        uv[2] = Vector2.up;
        uv[3] = Vector2.one;

        meshData.SetIndexBufferParams(trianglesIndexCount, IndexFormat.UInt16);
        NativeArray<ushort> trianglesIndices = meshData.GetIndexData<ushort>();
        trianglesIndices[0] = 0;
        trianglesIndices[1] = 2;
        trianglesIndices[2] = 1;
        trianglesIndices[3] = 1;
        trianglesIndices[4] = 2;
        trianglesIndices[5] = 3;

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, trianglesIndexCount));

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        GetComponent<MeshFilter>().mesh = mesh;
        //vertexAttributes.Dispose();
    }
}
