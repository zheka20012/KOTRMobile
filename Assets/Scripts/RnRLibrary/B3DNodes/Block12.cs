﻿using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block12 : BaseGroupNode,IBoundingSphere
    {
        /// <inheritdoc />
        public Block12(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            UNKNOWN = new float[4];

            for (int i = 0; i < 4; i++)
            {
                UNKNOWN[i] = reader.ReadSingle();
            }

            UNKNOWN1 = new int[2];

            for (int i = 0; i < 2; i++)
            {
                UNKNOWN1[i] = reader.ReadInt32();
            }

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

        public float[] UNKNOWN { get; set; }

        public int[] UNKNOWN1 { get; set; }
    }
}