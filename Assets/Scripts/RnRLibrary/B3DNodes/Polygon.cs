using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public struct Polygon : IBinaryReadable
    {
        [Flags]
        internal enum PolygonFlags : int
        {
            None = 0x0,
            HasIntensityVector = 0x1,
            HasUvs = 0x2,
            HasIntensity = 0x10,
            HasIntensityFloat = 0x20,
        }
        /// <inheritdoc />
        public void Read(BinaryReader reader)
        {
            //read face type
            PolygonFlags polygonFlags = (PolygonFlags)(ushort)(reader.ReadInt32() ^ 1); 

            reader.BaseStream.Seek(8, SeekOrigin.Current); //Skip unknown float and int
            //TODO: Unknown float and int in face declaration

            // Material index
            // TODO: Use this material or material from base declaration?
            MaterialIndex = reader.ReadInt32();

            var indicesCount = reader.ReadInt32();

            Indices = new int[indicesCount];
            UVS = new Dictionary<int, Vector2>();

            for (int i = 0; i < indicesCount; i++)
            {
                Indices[i] = reader.ReadInt32();

                if ((polygonFlags & PolygonFlags.HasUvs) != PolygonFlags.None)
                {
                    UVS.Add(Indices[i], new Vector2(reader.ReadSingle(), reader.ReadSingle()));
                }

                if ((polygonFlags & PolygonFlags.HasIntensity) != PolygonFlags.None)
                {
                    if ((polygonFlags & PolygonFlags.HasIntensityVector) != PolygonFlags.None)
                    {
                        if ((polygonFlags & PolygonFlags.HasIntensityFloat) != PolygonFlags.None)
                        {
                            reader.ReadStruct<Vector3>(); // Intensity Vector, currently unused
                        }
                    }
                    else if ((polygonFlags & PolygonFlags.HasIntensityFloat) != PolygonFlags.None)
                    {
                        reader.ReadSingle(); // Intensity float, unused too
                    }
                }
            }

        }

        public void Parse(ref Mesh outMesh)
        {
            var indices = outMesh.GetIndices(0).ToList();
            indices.AddRange(Indices);
            outMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        }

        private int MaterialIndex;

        private int[] Indices;
        private Dictionary<int, Vector2> UVS ;

    }
}