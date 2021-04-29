using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block9 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block9(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            TriggerOffset = reader.ReadStruct<Vector3>();
            TriggerDistance = reader.ReadSingle();

            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public Vector3 TriggerOffset { get; set; }

        public float TriggerDistance { get; set; }
    }
}