using System;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    /// <summary>
    /// TXR Texture format from king
    /// </summary>
    public partial class RnRTexture : IBinaryReadable
    {
        public struct PixelFormatMask : IBinaryReadable
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

            public KOTRTextureFormat GetTextureFormat()
            {
                if (BlueMask == 0x1F)
                {

                    if (GreenMask == 0x7E0 && RedMask == 0xF800 && AlphaMask == 0x0)
                    {
                        return KOTRTextureFormat.RGB565;
                    }

                    if (GreenMask == 0x3E0 && RedMask == 0x7C00 && AlphaMask == 0)
                    {
                        return KOTRTextureFormat.RGB555;
                    }
                }
                else if (BlueMask == 0xF && GreenMask == 0xF0 && RedMask == 0xF00 && AlphaMask == 0xF000)
                {
                    return KOTRTextureFormat.ARGB4444;
                }
                else if (BlueMask == 0xFF && GreenMask == 0xFF00 && RedMask == 0xFF0000)
                {
                    if (AlphaMask == 0)
                    {
                        return KOTRTextureFormat.RGB888;
                    }

                    return KOTRTextureFormat.RGBA8888;
                }

                Debug.Log($"Unknown texture format: R: 0x{RedMask:x}, G: 0x{GreenMask:x}, B: 0x{BlueMask:x}, A: 0x{AlphaMask:x}");
                return KOTRTextureFormat.Unknown;
            }
        }

        public Texture2D Texture
        {
            get
            {
                if (_InternalTexture == null)
                {
                    CreateTexture();
                }

                return _InternalTexture;
            }
        }

        private Texture2D _InternalTexture;
        private PixelFormatMask _PixelFormatMask;
        private byte[] _ImageData;
        private ushort Width, Height;

        /// <inheritdoc />
        public virtual void Read(BinaryReader reader)
        {
            long filePosition = reader.BaseStream.Position;

            reader.BaseStream.Seek(12, SeekOrigin.Current);

            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();
            int bitDepth = reader.ReadByte();

            reader.BaseStream.Seek(1, SeekOrigin.Current);

            if (bitDepth == 16)
            {
                if (reader.ReadBytes(4).GetString() != "LOFF")
                {
                    Debug.LogError("LOFF not found!");
                    return;
                }

                reader.ReadUInt32();
                uint offset = reader.ReadUInt32();

                _ImageData = reader.ReadBytes(Width * Height * 2);

                reader.BaseStream.Seek(offset+filePosition, SeekOrigin.Begin);

                string section = reader.ReadBytes(4).GetString();
                
                if (section != "PFRM")
                {
                    if (section == "LVMP")
                    {
                        reader.BaseStream.Seek(reader.ReadUInt32()+6, SeekOrigin.Current); //At now we don't need mipmaps

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
                    }
                    else
                    {
                        Debug.Log($"PFRM not found at pos: {reader.BaseStream.Position}!");
                        return;
                    }
                }

                _PixelFormatMask = reader.Read<PixelFormatMask>();
            }
        }

        private void CreateTexture()
        {
            TextureUtility.TextureBytesConverter converter;

            try
            {
                converter = TextureUtility.TextureBytesConverter.GetConverter(_PixelFormatMask.GetTextureFormat());
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                _InternalTexture = Texture2D.whiteTexture;
                return;
            }
            
            _InternalTexture = new Texture2D(Width, Height, converter.UnityTextureFormat, false);

            _InternalTexture.LoadRawTextureData(converter.FlipBytes(converter.ConvertBytes(_ImageData, _PixelFormatMask), Width, Height));

            _InternalTexture.Apply(false, false);

        }

    }
}