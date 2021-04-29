using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block12 : BaseGroupNode,IBoundingSphere
    {
        /// <inheritdoc />
        public Block12(NodeHeader header) : base(header)
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

            UNKNOWN1 = new int[2];

            for (int i = 0; i < 2; i++)
            {
                UNKNOWN1[i] = reader.ReadInt32();
            }

            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public float[] UNKNOWN { get; set; }

        public int[] UNKNOWN1 { get; set; }
    }
}