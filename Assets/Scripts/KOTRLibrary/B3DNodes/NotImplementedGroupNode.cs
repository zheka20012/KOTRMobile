
using System;
using System.IO;
using UnityEngine;

namespace KOTRLibrary.B3DNodes
{
    public class NotImplementedGroupNode : BaseGroupNode
    {
        /// <inheritdoc />
        public NotImplementedGroupNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            while (reader.ReadUInt32() != 333) ;

            reader.BaseStream.Seek(-8, SeekOrigin.Current);

            if (reader.ReadUInt32() == 444)
            {
                reader.BaseStream.Seek(-8, SeekOrigin.Current); 
            }

            Debug.Log(reader.ReadUInt32());

            reader.BaseStream.Seek(-8, SeekOrigin.Current);

            ReadChilds(reader);
        }
    }
}
