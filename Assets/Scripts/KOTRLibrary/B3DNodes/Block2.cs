using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block2 : BaseGroupNode, IBoundingSphere
    {

        //TODO: Get info about 4-floats value (1,0,0,0)
        public float[] UNKNOWN;
        /// <inheritdoc />
        public Block2(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            UNKNOWN = new float[4];

            for (int i = 0; i < 4; i++)
            {
                UNKNOWN[i] = reader.ReadSingle();
            }

            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}