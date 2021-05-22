using System.IO;

namespace RnRLibrary.B3DNodes
{
    public class NotImplementedNode : BaseNode
    {
        /// <inheritdoc />
        public NotImplementedNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            while (reader.ReadUInt32() != 555) ;

            reader.BaseStream.Seek(-4, SeekOrigin.Current);
        }
    }
}
