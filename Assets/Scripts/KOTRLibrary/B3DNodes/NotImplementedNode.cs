
using System;
using System.IO;

namespace KOTRLibrary.B3DNodes
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
            while (reader.ReadUInt32() != 555)
            {
                continue;
            }
        }
    }
}
