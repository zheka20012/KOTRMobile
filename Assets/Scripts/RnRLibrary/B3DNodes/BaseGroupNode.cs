using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public abstract class BaseGroupNode : INode
    {
        public string Name { get; }

        public List<INode> ChildNodes;

        public abstract void Read(BinaryReader reader);

        protected void ReadChilds(BinaryReader reader)
        {
            uint numChilds = reader.ReadUInt32();

            ChildNodes = new List<INode>();

            INode tempNode;

            for (uint i = 0; i < numChilds; i++)
            {
                //TODO: fix reading child nodes
                while (reader.ReadUInt32() != (int)Identifier.Block_Start) ;

                if (reader.ReadUInt32() != (int)Identifier.Block_Separator)
                {
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                }

                NodeHeader header = reader.Read<NodeHeader>();

                tempNode = BaseNode.GetNode(header);

                tempNode.Read(reader);

                ChildNodes.Add(tempNode);
            }
        }

        /// <inheritdoc />
        public abstract Transform ProcessNode(Transform parentTransform);

        public BaseGroupNode(NodeHeader header)
        {
            Name = header.Name;
        }
    }
}