using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    //3d-Object node
    public class Block7 : BaseGroupNode, IBoundingSphere
    {
        public struct Vertex
        {
            public Vector3 Position;
            public Vector2 Uv;

            public Vertex(BinaryReader reader)
            {
                Position = reader.ReadStruct<Vector3>();
                Uv = reader.ReadStruct<Vector2>();
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"pos:{Position}, uv={Uv}";
            }
        }

        /// <inheritdoc />
        public Block7(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            ObjectName = reader.Read32ByteString();

            int vertexCount = reader.ReadInt32();

            Vertices = new List<Vertex>();

            for (int i = 0; i < vertexCount; i++)
            {
                Vertices.Add(new Vertex(reader));
            }
                
            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string ObjectName { get; set; }

        public List<Vertex> Vertices { get; set; }
    }
}