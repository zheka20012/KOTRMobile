using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class Block8 : BaseNode, IBoundingSphere
    {
        public struct MeshFaceDecl : IBinaryReadable
        {
            public uint Type;
            public float UNKNOWN;
            public uint UNKNOWN1;
            public uint MaterialIndex;

            public List<MeshFace> Faces;

            public void Read(BinaryReader reader)
            {
                Type = reader.ReadUInt32();

                UNKNOWN = reader.ReadSingle();
                UNKNOWN1 = reader.ReadUInt32();

                MaterialIndex = reader.ReadUInt32();
                uint count = reader.ReadUInt32();

                Faces = new List<MeshFace>();

                for (uint i = 0; i < count; i++)
                {
                    switch (Type)
                    {
                        case 2:
                        case 3:
                        case 131:
                            Faces.Add(new MeshFace(reader.ReadUInt32(), Vector3.zero, reader.ReadStruct<Vector2>()));
                            break;
                        case 117:
                            Faces.Add(new MeshFace(0, Vector3.zero, reader.ReadStruct<Vector2>()));
                            break;
                        case 48:
                        case 51:
                        case 176:
                        case 179:
                            Faces.Add(new MeshFace(reader.ReadUInt32(), reader.ReadStruct<Vector3>(), Vector2.zero));
                            break;
                        case 50:
                        case 178:
                            Faces.Add(new MeshFace(reader.ReadUInt32(), reader.ReadStruct<Vector3>(), reader.ReadStruct<Vector2>()));
                            break;
                        default:
                            Faces.Add(new MeshFace(reader.ReadUInt32(), Vector3.zero, Vector2.zero));
                            break;
                    }
                }
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"Type:{Type}, MatIndex:{MaterialIndex}, Size:{Faces.Count}";
            }
        }

        public struct MeshFace
        {
            public uint Index;
            public Vector3 Position;
            public Vector2 Uv;

            public MeshFace(uint index, Vector3 position, Vector2 uv)
            {
                Index = index;
                Position = position;
                Uv = uv;
            }
        }
        /// <inheritdoc />
        public Block8(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            var count = reader.ReadUInt32();

            MeshFaces = new List<MeshFaceDecl>();

            for (int i = 0; i < count; i++)
            {
                MeshFaces.Add(reader.Read<MeshFaceDecl>());
            }
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
        
        public List<MeshFaceDecl> MeshFaces { get; set; }
    }
}