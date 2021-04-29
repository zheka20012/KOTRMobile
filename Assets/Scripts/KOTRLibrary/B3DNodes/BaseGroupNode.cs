using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class BaseGroupNode : BaseNode
    {
        public List<BaseNode> ChildNodes;

        protected void ReadChilds(BinaryReader reader)
        {
            int numChilds = reader.ReadInt32();

            ChildNodes = new List<BaseNode>();

            for (int i = 0; i < numChilds; i++)
            {
                    //TODO: fix reading child nodes
                    while (reader.ReadUInt32() != 333) ;

                    NodeHeader header = reader.Read<NodeHeader>();

                    BaseNode node = BaseNode.GetNode(header);

                    node.Read(reader);

                    ChildNodes.Add(node);
            }

        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            ReadChilds(reader);
        }

        /// <inheritdoc />
        public BaseGroupNode(NodeHeader header) : base(header)
        {
        }
    }
}