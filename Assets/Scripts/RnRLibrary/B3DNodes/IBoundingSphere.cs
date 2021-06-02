using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public interface IBoundingSphere
    {
        Vector3 Position { get; }
        float Radius { get;}
    }
}