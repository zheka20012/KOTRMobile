using System.IO;

namespace KOTRLibrary.B3DNodes
{
    public class Block1 : BaseNode
    {
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            reader.BaseStream.Seek(64, SeekOrigin.Current);
        }

        /// <inheritdoc />
        public Block1(NodeHeader header) : base(header)
        {
        }
    }
}