using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block20 : BaseNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block20(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            VertexCount = reader.ReadUInt32();
            UNKNOWN = reader.ReadUInt32();
            UNKNOWN1 = reader.ReadUInt32();
            Count = reader.ReadUInt32();

            Vertices = new List<Vector3>();

            for (int i = 0; i < VertexCount; i++)
            {
                Vertices.Add(reader.ReadVector3());
            }

            UNKNOWN2 = new List<float>();

            for (int i = 0; i < Count; i++)
            {
                UNKNOWN2.Add(reader.ReadSingle());
            }
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            return parentTransform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public uint VertexCount { get; set; }

        public uint UNKNOWN { get; set; }
        public uint UNKNOWN1 { get; set; }

        public uint Count { get; set; }

        public List<Vector3> Vertices { get; set; }

        public List<float> UNKNOWN2 { get; set; }
    }
}