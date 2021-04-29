using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block3 : BaseGroupNode, IBoundingSphere
    {


        /// <inheritdoc />
        public Block3(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            base.Read(reader);

        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}