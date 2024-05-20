using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes
{
    public interface IMeshStreams
    {
        // �޽� ������ �ʱ�ȭ
        void Setup(Mesh.MeshData data, Bounds bounds, int vertexCount, int indexCount);

        // ������ �޽��� ���� ���ۿ� �����ϴ� �۾�
        void SetVertex(int index, Vertex data);

        // �ε��� ����,
        void SetTriangle(int index, int3 triangle);
    }
}
