using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block33 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block33(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Z = reader.ReadInt32();

            Position2 = reader.ReadStruct<Vector3>();

            Colors = new float[12];

            for (int i = 0; i < 12; i++)
            {
                Colors[i] = reader.ReadSingle();
            }

            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3 Position2 { get; set; }
        public float[] Colors { get; set; }
    }
}