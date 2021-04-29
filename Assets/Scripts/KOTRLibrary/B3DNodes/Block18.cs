using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block18 : BaseNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block18(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            MatrixName = reader.Read32ByteString();
            AttachmentName = reader.Read32ByteString();
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string MatrixName { get; set; }
        public string AttachmentName { get; set; }
    }
}