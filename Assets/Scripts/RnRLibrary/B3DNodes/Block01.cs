using System.IO;

namespace RnRLibrary.B3DNodes
{
    public class Block01 : BaseNode
    {
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            reader.BaseStream.Seek(64, SeekOrigin.Current);
        }

        /// <inheritdoc />
        public Block01(NodeHeader header) : base(header)
        {
        }
    }
}