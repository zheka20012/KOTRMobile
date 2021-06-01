using System;
using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    /// <summary>
    /// Photoshop RAW 16-bit image format;
    /// </summary>
    public class RAWFile
    {
        private ushort[,] Grid;

        private ushort Width, Height;

        public static RAWFile OpenFile(string filePath, ushort width, ushort height)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogException(new FileNotFoundException());
                return null;
            }

            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                // Start reading only if we have much data
                if (reader.BaseStream.Length > width * height * 2) 
                {
                    Debug.LogError("RAW File doesn't have much data to fill grid!");
                    return null;
                }
                 
                //Create new file and fill values
                RAWFile file = new RAWFile();

                file.Width = width;
                file.Height = height;

                file.Grid = new ushort[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        file.Grid[i, j] = reader.ReadUInt16();
                    }
                }

                return file;
            }
        }

        public ushort this[ushort x, ushort y]
        {
            get
            {
                if (x > Width || y > Height)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return Grid[x, y];
            }
        }
    }
}