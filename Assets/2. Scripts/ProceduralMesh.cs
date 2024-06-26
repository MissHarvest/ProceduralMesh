using UnityEngine;
using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEditor;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    static MeshJobScheduleDelegate[] jobs =
    {
        MeshJob<SqureGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedSqureGrid, SingleStream>.ScheduleParallel,
        MeshJob<SharedTriangleGrid, SingleStream>.ScheduleParallel,
        MeshJob<MySharedTriangleGrid, SingleStream>.ScheduleParallel,
        MeshJob<PointyHexaGrid, SingleStream>.ScheduleParallel,
        MeshJob<UVSphere, SingleStream>.ScheduleParallel,
    };

    public float gizmoRadius = 0.01f;
    public float gizmoLength = 0.04f; 
    public enum MeshType
    {
        SquareGrid, 
        SharedSquareGrid,
        SharedTriangleGrid,
        MySharedTriangleGrid,
        PointyHexaGrid,
        UVSphere
    }

    [System.Flags]
    public enum GizmoMode 
    {
        Nothing = 0,
        Vertices = 1,
        Normals = 2,
        Tangents = 4
    }

    [SerializeField]
    GizmoMode gizmos;

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

    Vector3[] vertices, normals;
    Vector4[] tangents;

    private void Update()
    {
        GenerateMesh();
        enabled = false;

        vertices = mesh.vertices;
        this.normals = mesh.normals;
        this.tangents = mesh.tangents;
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

    private void OnDrawGizmos()
    {
        if (gizmos == GizmoMode.Nothing || mesh == null) return;

        for(int i = 0; i < this.vertices.Length; ++i)
        {
            Vector3 pos = transform.TransformPoint(this.vertices[i]);
            if ((gizmos & GizmoMode.Vertices) != 0)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(pos, gizmoRadius);
            }

            if ((gizmos & GizmoMode.Normals) != 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(pos, transform.TransformDirection(normals[i].normalized) * gizmoLength);
            }

            if ((gizmos & GizmoMode.Tangents) != 0)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(pos, transform.TransformDirection(tangents[i].normalized) * gizmoLength);
            }
        }
    }
}
