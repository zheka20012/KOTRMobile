using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block04 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string MatrixName;

        public string UNKNOWN;

        /// <inheritdoc />
        public Block04(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();
            MatrixName = reader.Read32ByteString();
            UNKNOWN = reader.Read32ByteString();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            throw new System.NotImplementedException();
        }
    }
}