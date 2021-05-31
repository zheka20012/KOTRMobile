using System;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace RnRLibrary
{
    internal class Texture16Bit : ITextureDeclaration
    {
        internal struct PixelFormatMask : IBinaryReadable
        {
            public uint RedMask;
            public uint GreenMask;
            public uint BlueMask;
            public uint AlphaMask;

            /// <inheritdoc />
            public void Read(BinaryReader reader)
            {
                var blockSize = reader.ReadUInt32() + reader.BaseStream.Position;

                RedMask = reader.ReadUInt32();
                GreenMask = reader.ReadUInt32();
                BlueMask = reader.ReadUInt32();
                AlphaMask = reader.ReadUInt32();

                reader.BaseStream.Seek(blockSize, SeekOrigin.Begin);
            }

            public RnRLibrary.TextureFormat GetTextureFormat()
            {
                if (BlueMask == 0x1F)
                {

                    if (GreenMask == 0x7E0 && RedMask == 0xF800 && AlphaMask == 0x0)
                    {
                        return RnRLibrary.TextureFormat.RGB565;
                    }

                    if (GreenMask == 0x3E0 && RedMask == 0x7C00 && AlphaMask == 0)
                    {
                        return RnRLibrary.TextureFormat.RGB555;
                    }
                }
                else if (BlueMask == 0xF && GreenMask == 0xF0 && RedMask == 0xF00 && AlphaMask == 0xF000)
                {
                    return RnRLibrary.TextureFormat.ARGB4444;
                }
                else if (BlueMask == 0xFF && GreenMask == 0xFF00 && RedMask == 0xFF0000)
                {
                    if (AlphaMask == 0)
                    {
                        return RnRLibrary.TextureFormat.RGB888;
                    }

                    return RnRLibrary.TextureFormat.RGBA8888;
                }

                Debug.Log($"Unknown texture format: R: 0x{RedMask:x}, G: 0x{GreenMask:x}, B: 0x{BlueMask:x}, A: 0x{AlphaMask:x}");
                return RnRLibrary.TextureFormat.Unknown;
            }
        }

        private PixelFormatMask _PixelFormatMask;

        private byte[] _ImageData;

        private int Width, Height;

        /// <inheritdoc />
        public void LoadTextureData(BinaryReader reader, int width, int height)
        {
            Width = width;
            Height = height;

            long filePosition = reader.BaseStream.Position - 18;

            if (reader.ReadBytes(4).GetString() != "LOFF")
            {
                Debug.LogError("LOFF not found!");
                return;
            }

            reader.ReadUInt32();
            uint offset = reader.ReadUInt32();

            _ImageData = reader.ReadBytes(width * height * 2);

            reader.BaseStream.Seek(offset + filePosition, SeekOrigin.Begin);

            string section = reader.ReadBytes(4).GetString();

            if (section != "PFRM")
            {
                if (section == "LVMP")
                {
                    reader.BaseStream.Seek(reader.ReadUInt32() + 6, SeekOrigin.Current); //At now we don't need mipmaps

#if false // TODO: Fix mipmaps load
                        var mipmapCount = reader.ReadUInt32();
                        var w = reader.ReadUInt32();
                        var h = reader.ReadUInt32();
                        var bit = reader.ReadUInt32();

                        if (bit == 2)
                        {
                            var mipmapsShorts = new List<ushort[]>();

                            for (int i = 0; i < mipmapCount; i++)
                            {
                                ushort[] data = new ushort[w*h];

                                for (int j = 0; j < data.Length; j++)
                                {
                                    data[j] = reader.ReadUInt16();
                                }

                                data = data.Reverse().ToArray();
                                mipmapsShorts.Add(data);


                                w /= 2;
                                h /= 2;
                            }

                            mipmaps = new List<byte[]>();

                            for (int i = 0; i < mipmapCount; i++)
                            {
                                mipmaps.Add(new byte[mipmapsShorts[i].Length * 2]);
                                Buffer.BlockCopy(mipmapsShorts[i], 0, mipmaps[i], 0, mipmaps[i].Length);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < mipmapCount; i++)
                            {
                                byte[] data = new byte[w * h];

                                for (int j = 0; j < data.Length; j++)
                                {
                                    data[j] = reader.ReadByte();
                                }

                                data = data.Reverse().ToArray();
                                mipmaps.Add(data);

                                w /= 2;
                                h /= 2;
                            }
                        }

                        reader.BaseStream.Seek(2, SeekOrigin.Current);
                        Debug.Log(reader.BaseStream.Position);
#endif
                }
                else
                {
                    Debug.Log($"PFRM not found at pos: {reader.BaseStream.Position}!");
                    return;
                }
            }

            _PixelFormatMask = reader.Read<PixelFormatMask>();
        }

        /// <inheritdoc />
        public void CreateTexture(out Texture2D texture)
        {
            TextureUtility.TextureBytesConverter converter;

            try
            {
                converter = TextureUtility.TextureBytesConverter.GetConverter(_PixelFormatMask.GetTextureFormat());
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                texture = Texture2D.whiteTexture;
                return;
            }

            texture = new Texture2D(Width, Height, converter.UnityTextureFormat, false);

            texture.LoadRawTextureData(converter.FlipBytes(converter.ConvertBytes(_ImageData, _PixelFormatMask), (ushort)texture.width, (ushort)texture.height));

            texture.Apply(false, false);
        }
    }
}