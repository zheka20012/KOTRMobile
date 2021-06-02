using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block23 : BaseNode
    {
        public struct Polygon : IBinaryReadable
        {
            public List<Vector3> Vertices;

            /// <inheritdoc />
            public void Read(BinaryReader reader)
            {
                uint vertexCount = reader.ReadUInt32();
                Vertices = new List<Vector3>();

                for (int i = 0; i < vertexCount; i++)
                {
                    Vertices.Add(reader.ReadStruct<Vector3>());
                }
            }
        }

        /// <inheritdoc />
        public Block23(NodeHeader header) : base(header)
        {
        }


        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            UNKNOWN = reader.ReadUInt32();

            SurfaceType = reader.ReadUInt32();

            var unknownCount = reader.ReadUInt32();

            UNKNOWN1 = new float[unknownCount];

            for (int i = 0; i < unknownCount; i++)
            {
                UNKNOWN1[i] = reader.ReadUInt32();
            }

            unknownCount = reader.ReadUInt32();

            Polygons = new List<Polygon>();

            for (int i = 0; i < unknownCount; i++)
            {
                Polygons.Add(reader.Read<Polygon>());
            }
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            throw new System.NotImplementedException();
        }

        public uint UNKNOWN { get; set; }

        public uint SurfaceType { get; set; }

        public float[] UNKNOWN1 { get; set; } 

        public List<Polygon> Polygons { get; set; }
    }
}