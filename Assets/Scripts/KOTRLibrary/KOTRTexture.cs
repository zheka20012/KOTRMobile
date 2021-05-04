using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KOTRLibrary
{
    /// <summary>
    /// TXR Texture format from king
    /// </summary>
    public partial class KOTRTexture : IBinaryReadable
    {
        protected struct PixelFormatMask : IBinaryReadable
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
        }

        public Texture2D Texture;

        /// <inheritdoc />
        public virtual void Read(BinaryReader reader)
        {
            long filePosition = reader.BaseStream.Position;

            reader.BaseStream.Seek(12, SeekOrigin.Current);

            short width = reader.ReadInt16();
            short height = reader.ReadInt16();
            int bitDepth = reader.ReadByte();

            reader.BaseStream.Seek(1, SeekOrigin.Current);

            if (bitDepth == 16)
            {
                if (reader.ReadBytes(4).GetString() != "LOFF")
                {
                    Debug.LogError("LOFF not found!");
                    return;
                }

                uint sectionSize = reader.ReadUInt32();
                uint offset = reader.ReadUInt32();

                Texture = new Texture2D(width, height, TextureFormat.ARGB4444, false);

                var imageData = reader.ReadBytes(width * height * 2);

                reader.BaseStream.Seek(offset+filePosition, SeekOrigin.Begin);

                string section = reader.ReadBytes(4).GetString();

                var mipmaps = new List<byte[]>();

                if (section != "PFRM")
                {
                    if (section == "LVMP")
                    {
                        reader.BaseStream.Seek(reader.ReadUInt32(), SeekOrigin.Current); //At now we don't need mipmaps

#if false
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
                        Debug.Log(reader.ReadBytes(4).GetString());
                    }
                    else
                    {
                        Debug.LogError($"PFRM not found at pos: {reader.BaseStream.Position}!");
                        return;
                    }
                }

                PixelFormatMask pfrm = reader.Read<PixelFormatMask>();

                TextureFormat outFormat = TextureFormat.RGB565;

                if (pfrm.RedMask == 0xF800 && pfrm.GreenMask == 0x7E0 && pfrm.BlueMask == 0x1F)
                {
                    outFormat = TextureFormat.RGB24;
                    imageData = GetRGBTexture(imageData, pfrm);
                }
                else if(pfrm.RedMask == 0xF00 && pfrm.GreenMask == 0xF0 && pfrm.BlueMask == 0xF && pfrm.AlphaMask == 0xF000)
                {
                    outFormat = TextureFormat.RGBA32;
                    imageData = GetARGBTexture(imageData, pfrm);
                }
                else if(pfrm.RedMask == 0x7c00 && pfrm.GreenMask == 0x3e0 && pfrm.BlueMask == 0x1f)
                {
                   outFormat = TextureFormat.RGB24;
				   imageData = Get555RGBTexture(imageData, pfrm);
                }

                Texture = new Texture2D(width, height, outFormat, false);

                Texture.LoadRawTextureData(imageData);

                Texture.Apply(false, false);
            }
        }

        protected byte[] GetRGBTexture(byte[] textureBytes, PixelFormatMask pixelFormatMask)
        {
            byte[] rgbTextureBytes = new byte[textureBytes.Length/2 * 3];

            for (int i = 0, j = 0; i < textureBytes.Length; i+=2)
            {
                short c = BitConverter.ToInt16(textureBytes, i);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.RedMask) >> 11) << 3);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.GreenMask) >> 5) << 2);
                rgbTextureBytes[j++] = (byte)((c & pixelFormatMask.BlueMask) << 3);
            }

            return rgbTextureBytes;
        }
		
		protected byte[] Get555RGBTexture(byte[] textureBytes, PixelFormatMask pixelFormatMask)
        {
            byte[] rgbTextureBytes = new byte[textureBytes.Length/2 * 3];

            for (int i = 0, j = 0; i < textureBytes.Length; i+=2)
            {
                short c = BitConverter.ToInt16(textureBytes, i);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.RedMask) >> 10)  << 3);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.GreenMask) >> 5) << 3);
                rgbTextureBytes[j++] = (byte)((c & pixelFormatMask.BlueMask) 		 << 3);
            }

            return rgbTextureBytes;
        }

        protected byte[] GetARGBTexture(byte[] textureBytes, PixelFormatMask pixelFormatMask)
        {
            byte[] rgbTextureBytes = new byte[textureBytes.Length / 2 * 4];

            for (int i = 0, j = 0; i < textureBytes.Length; i += 2)
            {
                 
                short c = BitConverter.ToInt16(textureBytes, i);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.RedMask) >> 8)  << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.GreenMask) >> 4) << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.BlueMask)      ) << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.AlphaMask) >> 12) << 4);

            }

            return rgbTextureBytes;
        }

        protected byte[] GetNativeARGB(byte[] textureBytes, PixelFormatMask pixelFormatMask)
        {
            byte[] rgbTextureBytes = new byte[textureBytes.Length / 2 * 4];

            for (int i = 0, j = 0; i < textureBytes.Length; i += 2)
            {

                short c = BitConverter.ToInt16(textureBytes, i);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.RedMask) >> 8) << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.GreenMask) >> 4) << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.BlueMask)) << 4);
                rgbTextureBytes[j++] = (byte)(((c & pixelFormatMask.AlphaMask) >> 12) << 4);

            }

            return rgbTextureBytes;
        }
    }
}