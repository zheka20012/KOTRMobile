using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class RoomNode : BaseGroupNode
    {
        /// <inheritdoc />
        public RoomNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform);

            EnumTree(_transform, file);

            return _transform;
        }
    }
}