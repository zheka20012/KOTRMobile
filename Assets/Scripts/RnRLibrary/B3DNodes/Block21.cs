﻿using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block21 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block21(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            Count = reader.ReadUInt32();

            UNKNOWN = reader.ReadUInt32();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform, false);

            EnumTree(_transform, file);

            return _transform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public uint Count { get; set; }
        public uint UNKNOWN { get; set; }


    }
}