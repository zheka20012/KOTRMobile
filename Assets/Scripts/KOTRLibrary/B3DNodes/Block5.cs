using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block5 : BaseGroupNode, IBoundingSphere
    {

        public string OtherName;
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();
            OtherName = reader.Read32ByteString();


            ReadChilds(reader);
        }

        /// <inheritdoc />
        public Block5(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}