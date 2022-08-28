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
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            RoomName = reader.Read32ByteString();

            Point1 = reader.ReadVector3();
            Point2 = reader.ReadVector3();

        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            return this.CreateObject(parentTransform);
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