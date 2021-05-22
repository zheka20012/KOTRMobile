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

        public List<float> Colors { get; private set; }

        public List<SectionInfo<Palette>> Palettes { get; private set; }
        public List<SectionInfo<SoundFile>> SoundFiles { get; private set; }
        public List<SectionInfo<RnRTexture>> TextureFiles { get; private set; }
        public List<SectionInfo<Mask>> MaskFiles { get; private set; }

        public List<RnRMaterial> Materials
        {
            get
            {
                if (!_MaterialsCreated)
                {
                    InitMaterials();
                }
                return _Materials;
            }
        }

        private bool _MaterialsCreated = false;

        private List<RnRMaterial> _Materials;

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

        private void InitMaterials()
        {
            for (int i = 0; i < _Materials.Count; i++)
            {
                _Materials[i].InitMaterial(this);
            }

            _MaterialsCreated = true;
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
            _Materials = new List<RnRMaterial>();

            for (uint i = 0; i < count; i++)
            {
                string materialData = reader.ReadCString();

                _Materials.Add(new RnRMaterial(materialData));
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