using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubMeshProceduralMesh : MonoBehaviour
{
    private void OnEnable()
    {
        int vertexAttributesCount = 4;
        int vertexCount = 4;
        int indexCount = 6;

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData1 = meshDataArray[0];

        // 각 속성을 정의해주어야 한다.
        NativeArray<VertexAttributeDescriptor> vertexAttributes =
            new NativeArray<VertexAttributeDescriptor>(
                vertexAttributesCount, Allocator.Temp);

        vertexAttributes[0] = new VertexAttributeDescriptor(
            VertexAttribute.Position, dimension: 3, stream: 0);
        vertexAttributes[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension:3, stream: 1);
        vertexAttributes[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension:4, stream: 2);
        vertexAttributes[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension:2, stream: 3);

        meshData1.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<Vector3> positions1 = meshData1.GetVertexData<Vector3>();
        positions1[0] = Vector3.zero;
        positions1[1] = Vector3.right;
        positions1[2] = Vector3.up;
        positions1[3] = new Vector3(1, 1, 0);

        var normals1 = meshData1.GetVertexData<Vector3>(1);
        normals1[0] = normals1[1] = normals1[2] = normals1[3] = Vector3.back;

        var tangents1 = meshData1.GetVertexData<Vector4>(2);
        tangents1[0] = tangents1[1] = tangents1[2] = tangents1[3] = new Vector4(1, 0, 0, -1);

        var uv1 = meshData1.GetVertexData<Vector2>(3);
        uv1[0] = Vector2.zero;
        uv1[1] = Vector2.right;
        uv1[2] = Vector2.up;
        uv1[3] = Vector3.one;

        // 인덱스 버퍼 채우기
        meshData1.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

        var indecies1 = meshData1.GetIndexData<ushort>();
        indecies1[0] = 0;
        indecies1[1] = 2;
        indecies1[2] = 1;
        indecies1[3] = 1;
        indecies1[4] = 2;
        indecies1[5] = 3;

        meshData1.subMeshCount = 2;

        meshData1.SetSubMesh(0, new SubMeshDescriptor(0, 3));
        meshData1.SetSubMesh(1, new SubMeshDescriptor(3, 3));

        var mesh = new Mesh
        {
            name = "SubMesh"
        };

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
