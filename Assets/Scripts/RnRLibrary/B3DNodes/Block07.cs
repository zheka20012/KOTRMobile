using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    //3d-Object node
    public class Block07 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block07(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            ObjectName = reader.Read32ByteString();

            int vertexCount = reader.ReadInt32();

            Vertices = new List<Vector3>();
            UVs = new List<Vector2>();

            for (int i = 0; i < vertexCount; i++)
            {
                Vertices.Add(reader.ReadVector3());
                UVs.Add(reader.ReadStruct<Vector2>());
            }

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            Transform _transform = this.CreateObject(parentTransform, false);

            if (Vertices != null)
            {
                var filter = _transform.gameObject.AddComponent<MeshFilter>();
                var renderer = _transform.gameObject.AddComponent<MeshRenderer>();
                renderer.sharedMaterial = new Material(Shader.Find("RnRBaseDiffuse"));

                Mesh newMesh = new Mesh();

                newMesh.SetVertices(Vertices);
                newMesh.SetUVs(0, UVs);

                filter.sharedMesh = newMesh;
            }

            EnumTree(_transform, file);

            return parentTransform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string ObjectName { get; set; }

        public List<Vector3> Vertices { get; set; }
        public List<Vector2> UVs { get; set; }
    }
}