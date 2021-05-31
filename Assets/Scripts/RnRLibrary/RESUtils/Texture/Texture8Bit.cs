using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    internal class Texture8Bit : ITextureDeclaration
    {
        private Color32[] ColorPalette;

        private byte[] ColorIndexes;

        private int Width, Height;

        /// <inheritdoc />
        public void LoadTextureData(BinaryReader reader, int width, int height)
        {
            Width = width;
            Height = height;

            reader.BaseStream.Seek(-16, SeekOrigin.Current);

            byte textureType = reader.ReadByte();

            ushort paletteEntry = reader.ReadUInt16();
            ushort paletteSize = reader.ReadUInt16();
            byte paletteItemSize = reader.ReadByte();

            reader.BaseStream.Seek(10, SeekOrigin.Current);

            ColorPalette = new Color32[paletteSize];

            for (int i = 0; i < ColorPalette.Length; i++)
            {
                ColorPalette[i] = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);
            }

            ColorIndexes = new byte[width * height];

            for (int i = 0; i < ColorIndexes.Length; i++)
            {
                ColorIndexes[i] = reader.ReadByte();
            }
        }

        /// <inheritdoc />
        public void CreateTexture(out Texture2D texture)
        {
            texture = new Texture2D(Width, Height, UnityEngine.TextureFormat.RGBA32, false);

            byte[] colors = new byte[Width * Height * 4];

            for (int i = 0, j = 0; i < ColorIndexes.Length; i++)
            {
                colors[j++] = ColorPalette[ColorIndexes[i]].r;
                colors[j++] = ColorPalette[ColorIndexes[i]].g;
                colors[j++] = ColorPalette[ColorIndexes[i]].b;
                colors[j++] = ColorPalette[ColorIndexes[i]].a;
            }

            texture.LoadRawTextureData(TextureUtility.FlipBytes(colors, (ushort)Width, (ushort)Height, 4));

            texture.Apply();
        }
    }
}