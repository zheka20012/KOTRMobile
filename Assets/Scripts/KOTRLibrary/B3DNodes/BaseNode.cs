using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KOTRLibrary.B3DNodes
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
            {0, typeof(Block0) },
            {1, typeof(Block1) },
            {2, typeof(Block2) },
            {3, typeof(Block3) },
            {4, typeof(Block4) },
            {5, typeof(Block5) },
            {7, typeof(Block7) },
            {8, typeof(Block8) },
            {9, typeof(Block9) },
            {10, typeof(Block10) },
            {12, typeof(Block12) },
            {18, typeof(Block18) },
            {19, typeof(BaseGroupNode) },
            {20, typeof(Block20) },
            {21, typeof(Block21) },
            {24, typeof(Block24) },
            {25, typeof(Block25) },
            {29, typeof(NotImplementedGroupNode) },
            {30, typeof(Block30) },
            {33, typeof(Block33) },
            {37, typeof(NotImplementedGroupNode) },
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