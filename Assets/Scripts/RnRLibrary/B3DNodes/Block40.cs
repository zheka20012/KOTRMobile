using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block40 : BaseNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block40(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            Empty = reader.Read32ByteString();
            GeneratorName = reader.Read32ByteString();

            Type = reader.ReadUInt32();

            UNKNOWN = reader.ReadSingle();

            UNKNOWN1 = new float[reader.ReadUInt32()];

            for (int i = 0; i < UNKNOWN1.Length; i++)
            {
                UNKNOWN1[i] = reader.ReadSingle();
            }
        }

        //TODO: implement tree generators
        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            return parentTransform;
        }

        public string Empty { get; set; }
        public string GeneratorName { get; set; }

        public uint Type { get; set; }

        public float UNKNOWN { get; set; }

        public float[] UNKNOWN1 { get; set; }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}