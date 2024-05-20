using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes
{
    public interface IMeshStreams
    {
        // 메쉬 데이터 초기화
        void Setup(Mesh.MeshData data, Bounds bounds, int vertexCount, int indexCount);

        // 정점을 메쉬의 정점 버퍼에 복사하는 작업
        void SetVertex(int index, Vertex data);

        // 인덱스 또한,
        void SetTriangle(int index, int3 triangle);
    }
}
