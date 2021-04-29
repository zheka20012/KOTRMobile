using System.IO;

namespace KOTRLibrary.B3DNodes
{
    public class Block0 : BaseNode
    {
        /// <inheritdoc />
        public Block0(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            reader.ReadBytes(44);
        }
    }
}