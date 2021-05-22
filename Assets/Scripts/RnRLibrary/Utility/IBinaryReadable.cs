using System.IO;

namespace RnRLibrary.Utility
{
    public interface IBinaryReadable
    {
        void Read(BinaryReader reader);
    }
}