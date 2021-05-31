using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block09 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block09(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            TriggerOffset = reader.ReadStruct<Vector3>();
            TriggerDistance = reader.ReadSingle();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            var _transform = new GameObject(Name).transform;

            EnumTree(_transform);

            return _transform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public Vector3 TriggerOffset { get; set; }

        public float TriggerDistance { get; set; }
    }
}