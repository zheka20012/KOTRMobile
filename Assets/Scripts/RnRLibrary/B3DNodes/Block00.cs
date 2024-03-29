﻿using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block00 : BaseNode
    {
        /// <inheritdoc />
        public Block00(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            reader.ReadBytes(44);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            return null;
        }
    }
}