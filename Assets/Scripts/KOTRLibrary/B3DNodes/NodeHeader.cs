using System.IO;

namespace KOTRLibrary.B3DNodes
{
    public struct NodeHeader : IBinaryReadable
    {
        public string Name;
        public uint Id;

        public void Read(BinaryReader reader)
        {
            Name = reader.Read32ByteString();
            Id = reader.ReadUInt32();
        }
    }
}