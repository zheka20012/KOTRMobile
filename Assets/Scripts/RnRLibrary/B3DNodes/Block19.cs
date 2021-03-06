using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block19 : BaseGroupNode
    {
        /// <inheritdoc />
        public Block19(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            throw new System.NotImplementedException();
        }
    }
}