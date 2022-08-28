using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
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
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            MatrixName = reader.Read32ByteString();
            AttachmentName = reader.Read32ByteString();
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform, true);

            

            return _transform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string MatrixName { get; set; }
        public string AttachmentName { get; set; }
    }
}