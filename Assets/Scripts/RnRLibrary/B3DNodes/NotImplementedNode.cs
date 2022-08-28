using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class NotImplementedNode : BaseNode
    {
        public uint nodeId;

        /// <inheritdoc />
        public NotImplementedNode(NodeHeader header) : base(header)
        {
            nodeId = header.Id;
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {


            while (reader.ReadUInt32() != 555) ;

            reader.BaseStream.Seek(-4, SeekOrigin.Current);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            throw new System.NotImplementedException($"Node with ID:{nodeId} is not yet defined!");
        }
    }
}
