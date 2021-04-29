using System.IO;

namespace KOTRLibrary
{
    public interface IBinaryReadable
    {
        void Read(BinaryReader reader);
    }
}