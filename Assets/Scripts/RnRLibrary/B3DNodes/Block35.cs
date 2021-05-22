using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block35 : BaseNode, IBoundingSphere
    {

        /// <inheritdoc />
        public Block35(NodeHeader header) : base(header)
        {
        }

        //TODO: normal implementation of mesh node reading
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            reader.ReadUInt32();

            MaterialIndex = reader.ReadUInt32();

            var indexCount = reader.ReadUInt32();
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public uint MaterialIndex { get; set; }

    }
}