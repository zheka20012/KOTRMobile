using RnRLibrary.Utility;

namespace RnRLibrary.B3DNodes
{
    public interface IPolygon : IBinaryReadable
    {
        uint[] Indices { get; }
        uint MaterialIndex { get; }
    }
}