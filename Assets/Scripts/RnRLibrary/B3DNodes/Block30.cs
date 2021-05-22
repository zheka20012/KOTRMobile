using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block30 : BaseNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block30(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            RoomName = reader.Read32ByteString();

            Point1 = reader.ReadStruct<Vector3>();
            Point2 = reader.ReadStruct<Vector3>();

        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string RoomName { get; set; }

        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
    }
}