﻿using System;
using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;

namespace RnRLibrary.B3DNodes
{
    public abstract class BaseNode : IBinaryReadable
    {
        public string Name;

        public BaseNode(NodeHeader header)
        {
            Name = header.Name;
        }

        public abstract void Read(BinaryReader reader);

        private static Dictionary<uint, Type> NodeTypes = new Dictionary<uint, Type>()
        {
            {0, typeof(Block00) },
            {1, typeof(Block01) },
            {2, typeof(Block02) },
            {3, typeof(Block03) },
            {4, typeof(Block04) },
            {5, typeof(Block05) },
            {7, typeof(Block07) },
            {8, typeof(NotImplementedNode) },
            {9, typeof(Block09) },
            {10, typeof(Block10) },
            {12, typeof(Block12) },
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
            {36, typeof(Block36) },
            {37, typeof(Block37) },
            {39, typeof(NotImplementedGroupNode) },
            {40, typeof(Block40) },
        };

        public static BaseNode GetNode(NodeHeader header)
        {
            if (NodeTypes.ContainsKey(header.Id))
            {
                return Activator.CreateInstance(NodeTypes[header.Id], header) as BaseNode;
            }

            return new NotImplementedNode(header);
        }
    }
}