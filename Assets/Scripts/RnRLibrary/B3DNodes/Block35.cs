using System;
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

            Polygons = new Polygon[indicesCount];   

            for (int i = 0; i < indicesCount; i++)
            {
                Polygons[i] = reader.Read<Polygon>();
            }
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            if (parentTransform.GetComponent<MeshFilter>() == null) // We not found mesh filter, that's wrong
            {
                Debug.LogError("Wrong b3d hierarchy! Can't find mesh vertex delaration!", parentTransform);
                return parentTransform;
            }

            MeshFilter filter = parentTransform.GetComponent<MeshFilter>();

            Mesh usedMesh = filter.sharedMesh;

            for (int i = 0; i < Polygons.Length; i++)
            {
                Polygons[i].Parse(ref usedMesh);
            }

            filter.sharedMesh = usedMesh;

            return parentTransform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; private set; }

        /// <inheritdoc />
        public float Radius { get; private set; }

        public uint MaterialIndex { get; private set; }

        public Polygon[] Polygons { get; private set; }
    }
}