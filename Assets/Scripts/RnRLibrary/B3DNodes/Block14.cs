using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block14 : BaseNode
    {
        /// <inheritdoc />
        public Block14(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            reader.BaseStream.Seek(11 * 4, SeekOrigin.Current);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            return parentTransform;
        }
    }
}