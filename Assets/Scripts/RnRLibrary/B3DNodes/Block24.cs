﻿using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block24 : BaseGroupNode
    {
        public Vector3[] Matrix { get; set; }
        public Vector3 Position { get; set; }
        public int IsShown { get; set; }
        /// <inheritdoc />
        public Block24(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Matrix = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                Matrix[i] = reader.ReadVector3();
            }

            Position = reader.ReadVector3();

            IsShown = reader.ReadInt32();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            Transform obj = this.CreateObject(parentTransform);

            EnumTree(obj, file);

            return parentTransform;
        }
    }
}