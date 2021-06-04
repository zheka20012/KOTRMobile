using System;
using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public abstract class BaseNode : INode, IBinaryReadable
    {
        public string Name { get; }

        public BaseNode(NodeHeader header)
        {
            Name = header.Name;
        }

        /// <inheritdoc />
        public abstract void Read(BinaryReader reader);

        /// <inheritdoc />
        public abstract Transform ProcessNode(Transform parentTransform);

        private static Dictionary<uint, Type> NodeTypes = new Dictionary<uint, Type>()
        {
            {0, typeof(Block00) },
            {1, typeof(Block01) },
            {2, typeof(Block02) },
            {3, typeof(Block03) },
            {4, typeof(Block04) },
            {5, typeof(Block05) },
            {7, typeof(Block07) },
            {8, typeof(Block08) },
            {9, typeof(Block09) },
            {10, typeof(Block10) },
            {12, typeof(Block12) },
            {14, typeof(Block14) },
            {18, typeof(Block18) },
            {19, typeof(Block19) },
            {20, typeof(Block20) },
            {21, typeof(Block21) },
            {23, typeof(Block23) },
            {24, typeof(Block24) },
            {25, typeof(Block25) },
            {29, typeof(NotImplementedGroupNode) },
            {30, typeof(Block30) },
            {33, typeof(Block33) },
            {35, typeof(Block35) },
            {36, typeof(Block36) },
            {37, typeof(Block37) },
            {39, typeof(NotImplementedGroupNode) },
            {40, typeof(Block40) },
        };

        public static INode GetNode(NodeHeader header)
        {
            if (NodeTypes.ContainsKey(header.Id))
            {
                return Activator.CreateInstance(NodeTypes[header.Id], header) as INode;
            }

            return new NotImplementedNode(header);
        }
    }
}