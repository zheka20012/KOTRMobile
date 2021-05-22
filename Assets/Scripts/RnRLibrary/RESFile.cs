using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
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

        public struct SectionInfo<T> : IBinaryReadable
        {
            public string Name;
            public uint Size;
            public T Item;

            /// <inheritdoc />
            public void Read(BinaryReader reader)
            {
                Name = reader.ReadCString();
                Size = reader.ReadUInt32();
            }
        }

        public List<float> Colors;

        public List<SectionInfo<Palette>> Palettes;
        public List<SectionInfo<SoundFile>> SoundFiles;
        public List<SectionInfo<RnRTexture>> TextureFiles;
        public List<SectionInfo<Mask>> MaskFiles;
        public List<Material> Materials;

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
                            file.ReadMaskFiles(reader, header.Count);
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
        }

        public void ReadMaterials(BinaryReader reader, uint count)
        {
            Materials = new List<Material>();

            for (uint i = 0; i < count; i++)
            {
                Material mat = new Material(Shader.Find("Legacy Shaders/Diffuse"));

                string[] materialData = reader.ReadCString().Split(new char[] {' '});

                mat.name = materialData[0];

                for (int j = 1; j < materialData.Length; j+=2)
                {
                    //TODO: Maybe add some abstraction...
                    switch (materialData[i])
                    {
                        case "col":
                            
                            break;
                        case "transp":
                            break;
                        case "tex":
                            break;
                    }
                }

                Materials.Add(mat);
            }
        }

        public void ReadSoundNames(BinaryReader reader, uint count)
        {
            List<string> soundsList = new List<string>();

            for (uint i = 0; i < count; i++)
            {
                soundsList.Add(reader.ReadCString());
            }
        }

        public void ReadPalettes(BinaryReader reader, uint count)
        {
            Palettes = new List<SectionInfo<Palette>>();

            for (int i = 0; i < count; i++)
            {
                var sInfo = reader.Read<SectionInfo<Palette>>();
                sInfo.Item = reader.Read<Palette>();
                Palettes.Add(sInfo);
            }
        }

        public void ReadSoundFiles(BinaryReader reader, uint count)
        {
            SoundFiles = new List<SectionInfo<SoundFile>>();

            for (int i = 0; i < count; i++)
            {
                var sInfo = reader.Read<SectionInfo<SoundFile>>();
                sInfo.Item = reader.Read<SoundFile>();
                SoundFiles.Add(sInfo);
            }
        }

        public void ReadTextureFiles(BinaryReader reader, uint count)
        {
            TextureFiles = new List<SectionInfo<RnRTexture>>();

            for (int i = 0; i < count; i++)
            {
               
                var sInfo = reader.Read<SectionInfo<RnRTexture>>();
                long tempPos = reader.BaseStream.Position; //TODO: Implement full TXR and MSK support
                sInfo.Item = reader.Read<RnRTexture>();
                TextureFiles.Add(sInfo);
                reader.BaseStream.Seek(sInfo.Size + tempPos, SeekOrigin.Begin);
            }
        }

        public void ReadMaskFiles(BinaryReader reader, uint count)
        {
            MaskFiles = new List<SectionInfo<Mask>>();

            for (int i = 0; i < count; i++)
            {
                var sInfo = reader.Read<SectionInfo<Mask>>();
                long tempPos = reader.BaseStream.Position; //TODO: Implement full MSK support
                //sInfo.Item = reader.Read<KOTRTexture>();
                MaskFiles.Add(sInfo);
                reader.BaseStream.Seek(sInfo.Size + tempPos, SeekOrigin.Begin);
            }
        }
    }
}