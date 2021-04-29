using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block4 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string MatrixName;

        public string UNKNOWN;

        /// <inheritdoc />
        public Block4(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();
            MatrixName = reader.Read32ByteString();
            UNKNOWN = reader.Read32ByteString();

            base.Read(reader);
        }


    }
}