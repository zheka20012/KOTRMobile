using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block36 : BaseGroupNode, IBoundingSphere
    {

        /// <inheritdoc />
        public Block36(NodeHeader header) : base(header)
        {
        }

        //TODO: normal implementation of mesh node reading
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            Name1 = reader.Read32ByteString();
            Name2 = reader.Read32ByteString();

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
                        vertices.Add(reader.ReadStruct<Vector3>());
                        uv.Add(reader.ReadStruct<Vector2>());
                        normals.Add(reader.ReadStruct<Vector3>());
                    }
                    break;
                }
                case 3:
                {
                    vertices = new List<Vector3>();
                    normals = new List<Vector3>();

                    for (int i = 0; i < vertexCount; i++)
                    {
                        vertices.Add(reader.ReadStruct<Vector3>());
                        //uv.Add(reader.ReadStruct<Vector2>());
                        normals.Add(reader.ReadStruct<Vector3>());
                    }
                }
                break;
                case 4: //position + uv
                {
                    vertices = new List<Vector3>();
                    uv = new List<Vector2>();

                    for (int i = 0; i < vertexCount; i++)
                    {
                        vertices.Add(reader.ReadStruct<Vector3>());
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
                        vertices.Add(reader.ReadStruct<Vector3>());
                        uv.Add(reader.ReadStruct<Vector2>());
                        uv1.Add(reader.ReadStruct<Vector2>());
                        normals.Add(reader.ReadStruct<Vector3>());
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
                        vertices.Add(reader.ReadStruct<Vector3>());
                        uv.Add(reader.ReadStruct<Vector2>());
                        uv1.Add(reader.ReadStruct<Vector2>());
                        reader.BaseStream.Seek(8, SeekOrigin.Current);
                        normals.Add(reader.ReadStruct<Vector3>());
                    }
                    break;
                }
            }

            base.Read(reader);
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public string Name1 { get; set; }
        public string Name2 { get; set; }

        public List<Vector3> vertices { get; set; }
        public List<Vector2> uv { get; set; }
        public List<Vector2> uv1 { get; set; }
        public List<Vector3> normals { get; set; }
    }
}