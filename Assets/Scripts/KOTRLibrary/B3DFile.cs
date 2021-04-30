using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KOTRLibrary.B3DNodes;
using UnityEngine;

namespace KOTRLibrary
{
    [Serializable]
    public class B3DFile
    {
        private static byte[] B3DHeader = new byte[4] {98, 51, 100, 0}; //b3d signature with null-terminating character

        private enum Identifier
        {
            Other = 0,
            Chunk_Start = 111,
            Chunk_End = 222,
            Block_Start = 333,
            Block_Separator = 444,
            Block_End = 555,
        }

        public List<string> MaterialLibrary;

        public List<BaseNode> Nodes;

        public static B3DFile OpenFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                
                if (!reader.ReadBytes(4).SequenceEqual(B3DHeader))
                {
                    throw new InvalidDataException($"File is not b3d!");
                }

                B3DFile file = new B3DFile();

                //Skipped unused information
                //TODO: is it really unused???
                reader.BaseStream.Seek(4, SeekOrigin.Current);
                uint materialListOffset = reader.ReadUInt32();
                reader.BaseStream.Seek(4, SeekOrigin.Current);
                uint dataOffset = reader.ReadUInt32();

                //Read materials
                //Jump to material definition
                reader.BaseStream.Seek(materialListOffset * 4, SeekOrigin.Begin);

                uint materialLibrarySize = reader.ReadUInt32();

                file.MaterialLibrary = new List<string>();

                for (uint i = 0; i < materialLibrarySize; i++)
                {
                    file.MaterialLibrary.Add(reader.Read32ByteString());
                }

                //Read Data
                //Jump to data definition
                reader.BaseStream.Seek(dataOffset * 4, SeekOrigin.Begin);

                if (reader.ReadUInt32() == 111)
                {
                    file.Nodes = new List<BaseNode>();

                    uint ident;

                    while ((ident = reader.ReadUInt32()) != 222)
                    {
                        if (ident == 333)
                        {
                                NodeHeader header = reader.Read<NodeHeader>();

                                BaseNode node = BaseNode.GetNode(header);

                                node.Read(reader);

                                file.Nodes.Add(node);
                        }
                    }
                }

                return file;
            }

            throw new NotImplementedException();
        }
    }
}