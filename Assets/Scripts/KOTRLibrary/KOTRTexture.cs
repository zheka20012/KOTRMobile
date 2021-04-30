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

        public Texture2D Texture;

        /// <inheritdoc />
        public void Read(BinaryReader reader)
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

                var imageData  = new ushort[width*height];

                for (int i = 0; i < imageData.Length; i++)
                {
                    imageData[i] = reader.ReadUInt16();
                }

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
                        reader.BaseStream.Seek(4, SeekOrigin.Current);
                    }
                    else
                    {
                        Debug.LogError($"PFRM not found at pos: {reader.BaseStream.Position}!");
                        return;
                    }
                }



                uint PFRMsectionSize = reader.ReadUInt32();

                reader.BaseStream.Seek(12, SeekOrigin.Current); //Skip unused bitfields, we interested only in alpha

                uint alphaMask = reader.ReadUInt32();

                reader.BaseStream.Seek(PFRMsectionSize-16, SeekOrigin.Current);

                Texture = new Texture2D(width, height, alphaMask == 0 ? TextureFormat.RGB565 : TextureFormat.ARGB4444, false);

                imageData = imageData.Reverse().ToArray();

                var imageBytes = new byte[imageData.Length * 2];

                Buffer.BlockCopy(imageData, 0, imageBytes, 0, imageBytes.Length);

                Texture.LoadRawTextureData(imageBytes);

                Texture.Apply(true, false);
            }
        }
    }
}