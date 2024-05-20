using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUint16
    {
        public ushort a, b, c;
        public static implicit operator TriangleUint16(int3 t) => new TriangleUint16
        {
            a = (ushort)t.x,
            b = (ushort)t.y,
            c = (ushort)t.z,
        };
    }
}
