using System.IO;
using UnityEngine;

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
        public override Transform ProcessNode(Transform parentTransform)
        {
            return null;
        }

        /// <inheritdoc />
        public Block01(NodeHeader header) : base(header)
        {
        }
    }
}