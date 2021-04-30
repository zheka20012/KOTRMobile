using System.IO;
using UnityEngine;

namespace KOTRLibrary
{
    public class SoundFile : IBinaryReadable
    {
        private byte[] WAVData;

        /// <inheritdoc />
        public void Read(BinaryReader reader)
        {
            long offset = reader.BaseStream.Position;

            reader.BaseStream.Seek(4, SeekOrigin.Current);

            var size = reader.ReadUInt32();

            WAVData = new byte[size + 8];

            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            WAVData = reader.ReadBytes(WAVData.Length);
        }

        public AudioClip CreateAudioClip()
        {
            return WavUtility.ToAudioClip(WAVData);
        }

        public byte[] GetBytes()
        {
            return WAVData;
        }
    }
}