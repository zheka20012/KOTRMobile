using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block37 : BaseGroupNode, IBoundingSphere
    {

        /// <inheritdoc />
        public Block37(NodeHeader header) : base(header)
        {
        }

        //TODO: normal implementation of mesh node reading
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            CollisionName = reader.Read32ByteString();

            var vertexType = reader.ReadUInt32();

            var vertexCount = reader.ReadUInt32();

            switch (vertexType)
            {
                case 2: //position + uv + normal
                    {
                        vertices = new List<Vector3>();
                        normals = new List<Vector3>();
                        uv = new List<Vector2>();

                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices.Add(reader.ReadVector3());
                            uv.Add(reader.ReadStruct<Vector2>());
                            normals.Add(reader.ReadVector3());
                        }
                        break;
                    }
                case 3:
                    {
                        vertices = new List<Vector3>();
                        normals = new List<Vector3>();

                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices.Add(reader.ReadVector3());
                            //uv.Add(reader.ReadStruct<Vector2>());
                            normals.Add(reader.ReadVector3());
                        }
                    }
                    break;
                case 4: //position + uv
                    {
                        vertices = new List<Vector3>();
                        uv = new List<Vector2>();

                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices.Add(reader.ReadVector3());
                            uv.Add(reader.ReadStruct<Vector2>());
                            reader.ReadSingle();
                        }
                        break;
                    }

                case 258:
                case 515:
                    //position + uv + uv1 + normal
                    {
                        vertices = new List<Vector3>();
                        normals = new List<Vector3>();
                        uv = new List<Vector2>();
                        uv1 = new List<Vector2>();

                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices.Add(reader.ReadVector3());
                            uv.Add(reader.ReadStruct<Vector2>());
                            uv1.Add(reader.ReadStruct<Vector2>());
                            normals.Add(reader.ReadVector3());
                        }
                        break;
                    }

                case 514: //position + uv + uv1 + unknown + normal
                    {
                        vertices = new List<Vector3>();
                        normals = new List<Vector3>();
                        uv = new List<Vector2>();
                        uv1 = new List<Vector2>();

                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices.Add(reader.ReadVector3());
                            uv.Add(reader.ReadStruct<Vector2>());
                            uv1.Add(reader.ReadStruct<Vector2>());
                            reader.BaseStream.Seek(8, SeekOrigin.Current);
                            normals.Add(reader.ReadVector3());
                        }
                        break;
                    }
                default:
                    break;
            }

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            Transform _transform = this.CreateObject(parentTransform, false);

            if (vertices != null)
            {
                var filter = _transform.gameObject.AddComponent<MeshFilter>();
                var renderer = _transform.gameObject.AddComponent<MeshRenderer>();
                renderer.sharedMaterial = new Material(Shader.Find("RnRBaseDiffuse"));

                filter.sharedMesh = new Mesh();

                filter.sharedMesh.SetVertices(vertices);
                if (uv != null) filter.sharedMesh.uv = uv.ToArray();
                if (normals != null) filter.sharedMesh.SetNormals(normals);
            }

            EnumTree(_transform, file);

            return _transform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string CollisionName { get; set; }

        public List<Vector3> vertices { get; set; }
        public List<Vector2> uv { get; set; }
        public List<Vector2> uv1 { get; set; }
        public List<Vector3> normals { get; set; }
    }
}