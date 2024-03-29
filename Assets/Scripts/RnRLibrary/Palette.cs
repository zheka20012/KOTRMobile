﻿using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    public class Palette : IBinaryReadable
    {
        public Color[] PaletteColors;
        
        /// <inheritdoc />
        public void Read(BinaryReader reader)
        {
            if (reader.ReadCString() != "PLM")
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                Debug.LogError($"NOT PALETTE??? Header: {reader.ReadCString()}");
            }

            long fileSizeWithOffset = reader.ReadUInt32();
            fileSizeWithOffset += reader.BaseStream.Position;

            while (reader.BaseStream.Position < fileSizeWithOffset)
            {
                switch (System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4)))
                {
                    case "PALT": //Palette
                    {
                        reader.ReadUInt32();
                        ReadPalette(reader);
                    } break;
                }
            }

        }

        public void ReadPalette(BinaryReader reader)
        {

            PaletteColors = new Color[256];

            for (int i = 0; i < 256; i++)
            {
                PaletteColors[i] = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);
            }
        }


    }
}