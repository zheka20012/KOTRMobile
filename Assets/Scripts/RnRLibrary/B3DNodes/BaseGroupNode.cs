using System.Collections.Generic;
using System.IO;
using RnRLibrary.Utility;

namespace RnRLibrary.B3DNodes
{
    public class BaseGroupNode : BaseNode
    {
        public List<BaseNode> ChildNodes;


        protected void ReadChilds(BinaryReader reader)
        {
            uint numChilds = reader.ReadUInt32();

            ChildNodes = new List<BaseNode>();

            BaseNode tempNode;

            for (uint i = 0; i < numChilds; i++)
            {
                //TODO: fix reading child nodes
                while (reader.ReadUInt32() != 333) ;

                if (reader.ReadUInt32() != 444)
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