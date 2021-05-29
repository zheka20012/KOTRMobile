using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block35 : BaseNode, IBoundingSphere
    {

        /// <inheritdoc />
        public Block35(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            reader.ReadUInt32();

            MaterialIndex = reader.ReadUInt32();

            var indicesCount = reader.ReadUInt32();

            Polygons = new List<IPolygon>();

            for (int i = 0; i < indicesCount; i++)
            {
                int polygonType = reader.ReadInt32();

                IPolygon polygon = PolygonResolver.ResolvePolygon(polygonType);

                polygon.Read(reader);

                Polygons.Add(polygon);
            }
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public uint MaterialIndex { get; set; }

        public List<IPolygon> Polygons { get; private set; }
    }
}