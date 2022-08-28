using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    /// <summary>
    /// Object Collision parameters
    /// </summary>
    public class Block23 : BaseNode
    {
        public struct Polygon : IBinaryReadable
        {
            public List<Vector3> Vertices;

            /// <inheritdoc />
            public void Read(BinaryReader reader)
            {
                uint submeshCount = reader.ReadUInt32();
                Vertices = new List<Vector3>();

                for (int i = 0; i < submeshCount; i++)
                {
                    Vertices.Add(reader.ReadVector3());
                }
            }

            public void ParseMesh(ref Mesh mesh)
            {
                List<Vector3> vertices = mesh.vertices.ToList();

                vertices.AddRange(Vertices);

                mesh.vertices = vertices.ToArray();
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
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            MeshCollider _collider = parentTransform.gameObject.GetOrAddComponent<MeshCollider>();

            Mesh workMesh = _collider.sharedMesh ? _collider.sharedMesh : new Mesh();

            workMesh.subMeshCount++;

            int subMesh = workMesh.subMeshCount - 1;

            for (int i = 0; i < Polygons.Count; i++)
            {
                Polygons[i].ParseMesh(ref workMesh);
            }

            int[] tris = new int[workMesh.vertexCount];

            int num = 0;

            for (int i = 0; i < workMesh.vertexCount; i++)
            {
                tris[i] = num++;
            }

            workMesh.SetIndices(tris, MeshTopology.Quads, subMesh);

            _collider.sharedMesh = workMesh;

            return parentTransform;
        }

        public uint UNKNOWN { get; set; }

        public uint SurfaceType { get; set; }

        public float[] UNKNOWN1 { get; set; } 

        public List<Polygon> Polygons { get; set; }
    }
}