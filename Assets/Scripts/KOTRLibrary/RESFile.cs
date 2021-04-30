using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace KOTRLibrary
{
    public class RESFile
    {
        private enum SectionType
        {
            COLORS,
            TEXTUREFILES,
            PALETTEFILES,
            SOUNDFILES,
            BACKFILES,
            MASKFILES,
            MATERIALS,
            SOUNDS
        }

        private struct SectionHeader
        {
            public SectionType Type;
            public uint Count;

            public SectionHeader(BinaryReader reader)
            {
                string[] results = reader.ReadCString().Split(' ');

                Type =  (SectionType)Enum.Parse(typeof(SectionType),results[0],true);

                Count = UInt32.Parse(results[1]);
            }
        }

        public struct SectionInfo : IBinaryReadable
        {
            public string Name;
            public uint Size;
            public IResourceItem Item;

            /// <inheritdoc />
            public void Read(BinaryReader reader)
            {
                Name = reader.ReadCString();
                Size = reader.ReadUInt32();
            }
        }

        public List<float> Colors;

        public List<SectionInfo> Palettes;
        public List<SectionInfo> SoundFiles;
        public List<SectionInfo> TextureFiles;

        public static RESFile OpenFile(string filePath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                RESFile file = new RESFile();

                long fileLength = reader.BaseStream.Length;

                while (reader.BaseStream.Position < fileLength) //TODO: Caching
                {
                    SectionHeader header = new SectionHeader(reader);

                    switch (header.Type)
                    {
                        case SectionType.COLORS:
                            file.ReadColors(reader, header.Count);
                            break;
                        case SectionType.TEXTUREFILES:
                            file.ReadTextureFiles(reader, header.Count);
                            break;
                        case SectionType.PALETTEFILES:
                            file.ReadPalettes(reader, header.Count);
                            break;
                        case SectionType.SOUNDFILES:
                            file.ReadSoundFiles(reader, header.Count);
                            break;
                        case SectionType.BACKFILES:
                            break;
                        case SectionType.MASKFILES:
                            break;
                        case SectionType.MATERIALS:
                            file.ReadMaterials(reader, header.Count);
                            break;
                        case SectionType.SOUNDS:
                            file.ReadSoundNames(reader, header.Count);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return file;
            }
        }

        public void ReadColors(BinaryReader reader, uint count)
        {
            Colors = new List<float>();

            for (uint i = 0; i < count; i++)
            {
                Colors.Add(Single.Parse(reader.ReadCString(), NumberStyles.Any, CultureInfo.InvariantCulture));
            }

            Debug.Log("Colors Loaded!");
        }

        public void ReadMaterials(BinaryReader reader, uint count)
        {
            List<string> materialsList = new List<string>();

            for (uint i = 0; i < count; i++)
            {
                materialsList.Add(reader.ReadCString());
            }

            Debug.Log("Materials Loaded!");
        }

        public void ReadSoundNames(BinaryReader reader, uint count)
        {
            List<string> soundsList = new List<string>();

            for (uint i = 0; i < count; i++)
            {
                soundsList.Add(reader.ReadCString());
            }

            Debug.Log("Sound Names Loaded!");
        }

        public void ReadPalettes(BinaryReader reader, uint count)
        {
            Palettes = new List<SectionInfo>();

            for (int i = 0; i < count; i++)
            {
                var sInfo = reader.Read<SectionInfo>();
                sInfo.Item = reader.Read<Palette>();
                Palettes.Add(sInfo);
            }

            Debug.Log("Palettes Loaded!");
        }

        public void ReadSoundFiles(BinaryReader reader, uint count)
        {
            SoundFiles = new List<SectionInfo>();

            for (int i = 0; i < count; i++)
            {
                var sInfo = reader.Read<SectionInfo>();
                sInfo.Item = reader.Read<SoundFile>();
                SoundFiles.Add(sInfo);
            }

            Debug.Log("Sound Files Loaded!");
        }

        public void ReadTextureFiles(BinaryReader reader, uint count)
        {
            TextureFiles = new List<SectionInfo>();

            for (int i = 0; i < count; i++)
            {
               
                var sInfo = reader.Read<SectionInfo>();
                long tempPos = reader.BaseStream.Position; //TODO: Implement full TXR and MSK support
                sInfo.Item = reader.Read<KOTRTexture>();
                TextureFiles.Add(sInfo);
                reader.BaseStream.Seek(sInfo.Size + tempPos, SeekOrigin.Begin);
            }

            Debug.Log("Texture Files Loaded!");
        }
    }
}