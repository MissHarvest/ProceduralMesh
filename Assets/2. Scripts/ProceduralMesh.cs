using UnityEngine;
using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine.Rendering;
using Unity.VisualScripting;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    static MeshJobScheduleDelegate[] jobs =
    {
        MeshJob<SqureGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedSqureGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedTriangleGrid, SingleStream>.ScheduleParallel
    };

    public enum MeshType
    {
        SquareGrid, 
        SharedSquareGrid,
        SharedTriangleGrid,
    }
    [SerializeField]
    MeshType meshType;

    Mesh mesh;

    [SerializeField, Range(1, 50)]
    int resolution = 1;

    private void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        //GenerateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        GenerateMesh();
        enabled = false;
    }

    private void OnValidate()
    {
        enabled = true;
    }

    private void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        //MeshJob<SqureGrid, MultiStream>.ScheduleParallel(
        //    mesh, meshData, resolution, default).Complete();

        jobs[(int)meshType](mesh, meshData, resolution, default).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
    }
}
