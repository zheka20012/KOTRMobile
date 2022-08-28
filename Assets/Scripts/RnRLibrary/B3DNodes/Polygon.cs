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
            flags = (PolygonFlags)(ushort)(reader.ReadInt32() ^ 1);

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

                if ((flags & PolygonFlags.HasUvs) != PolygonFlags.None)
                {
                    UVS.Add(Indices[i], reader.ReadStruct<Vector2>());
                }

                if ((flags & PolygonFlags.HasIntensity) != PolygonFlags.None)
                {
                    if ((flags & PolygonFlags.HasIntensityVector) != PolygonFlags.None)
                    {
                        if ((flags & PolygonFlags.HasIntensityFloat) != PolygonFlags.None)
                        {
                            reader.ReadVector3(); // Intensity Vector, currently unused
                        }
                    }
                    else if ((flags & PolygonFlags.HasIntensityFloat) != PolygonFlags.None)
                    {
                        reader.ReadSingle(); // Intensity float, unused too
                    }
                }
            }

        }

        public void Parse(ref Mesh outMesh, int subMesh)
        {
            List<int> newIndices = new List<int>();

            for (int i = 0; i < Indices.Length - 2; i++)
            {
                if (i % 2 == 0)
                {
                    newIndices.Add(Indices[i]);
                    newIndices.Add(Indices[i + 2]);
                    newIndices.Add(Indices[i + 1]);
                }
                else
                {
                    if ((flags == PolygonFlags.None) ||
                       (flags.HasFlag(PolygonFlags.HasIntensityVector)) ||
                        (flags.HasFlag(PolygonFlags.HasIntensityVector)))
                    {
                        newIndices.Add(Indices[i + 1]);
                        newIndices.Add(Indices[i + 2]);
                        newIndices.Add(Indices[i]);
                    }
                    else
                    {
                        newIndices.Add(Indices[i + 1]);
                        newIndices.Add(Indices[i + 2]);
                        newIndices.Add(Indices[0]);
                    }

                }
            }

            var indices = outMesh.GetIndices(subMesh).ToList();
            indices.AddRange(newIndices);
            outMesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, subMesh);

        }

        private int MaterialIndex;
        private PolygonFlags flags;
        private int[] Indices;
        private Dictionary<int, Vector2> UVS;

    }
}