using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    public class Mask : RnRTexture
    {
        private Palette Palette;
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            if (reader.ReadBytes(4).GetString() != "MS16")
            {
                Debug.LogError("File is not MSK file!");
                return;
            }

            ushort width = reader.ReadUInt16();
            ushort height = reader.ReadUInt16();

            Palette = new Palette();
            Palette.ReadPalette(reader);


        }
    }
}