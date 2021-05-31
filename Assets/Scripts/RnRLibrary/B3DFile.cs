using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.B3DNodes;
using RnRLibrary.Utility;
using UnityEditor.Experimental.UIElements.GraphView;

namespace RnRLibrary
{
    [Serializable]
    public class B3DFile
    {
        private static byte[] B3DHeader = new byte[4] { 98, 51, 100, 0 }; //b3d signature with null-terminating character

        public List<string> MaterialLibrary;

        public List<INode> Nodes;

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

                if (reader.ReadUInt32() == (int)Identifier.Chunk_Start)
                {
                    file.Nodes = new List<INode>();

                    uint ident;

                    while ((ident = reader.ReadUInt32()) != (int)Identifier.Chunk_End)
                    {
                        //if(ident == 444) continue; // Unused???

                        if (ident == (int)Identifier.Block_Start)
                        {
                            NodeHeader header = reader.Read<NodeHeader>();

                            INode node = BaseNode.GetNode(header);

                            node.Read(reader);

                            file.Nodes.Add(node);
                        }
                    }
                }

                return file;
            }

            throw new NotImplementedException();
        }

        public INode Find(string name)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Name == name)
                {
                    return Nodes[i];
                }
            }

            return null;
        }

        public INode this[int index]
        {
            get
            {
                if (index > Nodes.Count)
                {
                    return null;
                }

                return Nodes[index];
            }
        }

        public INode this[string name]
        {
            get
            {
                return Find(name);
            }
        }
    }
}