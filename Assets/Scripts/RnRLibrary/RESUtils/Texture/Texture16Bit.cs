using System;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace RnRLibrary
{
    internal class Texture16Bit : ITextureDeclaration
    {
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