using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshes
{
    public interface IMeshGenerator
    {
        void Excutes<S>(int i, S stream) where S : struct, IMeshStreams;

        int VertexCount { get; }
        int IndexCount { get; }
        int JobLength { get; }
        Bounds Bounds { get; }
        int Resolution { get; set; }
    }
}
