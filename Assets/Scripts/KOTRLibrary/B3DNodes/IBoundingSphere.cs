using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public interface IBoundingSphere
    {
        Vector3 Position { get; set; }
        float Radius { get; set; }
    }
}