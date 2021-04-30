using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace KOTRLibrary
{
    public static class BinaryReaderUtility
    {
        private static readonly string _NullTerminator = new string((char) 0, 1);
        /// <summary>
        /// Reads 32 bit string and removes all null terminators;
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string Read32ByteString(this BinaryReader reader)
        {
            string result = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(32));
            
            return result.Replace(_NullTerminator, "");
        }

        public static string GetString(this byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static string ReadCString(this BinaryReader reader)
        {
            List<byte> stringChars = new List<byte>();

            byte c;
            while ((c = reader.ReadByte()) != 0)
            {
                stringChars.Add(c);
            }

            return System.Text.Encoding.UTF8.GetString(stringChars.ToArray());
        }

        public static T ReadStruct<T>(this BinaryReader reader)
        {
            byte[] buffer = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            handle.Free();

            return result;
        }

        public static T Read<T>(this BinaryReader reader) where T : IBinaryReadable, new()
        {
            T result = new T();
            result.Read(reader);
            return result;
        }

        public static T ReadEnum<T>(this BinaryReader reader)
        {
            int value = reader.ReadInt32();

            var name = Enum.GetName(typeof(T), value);

            if (name == null)
            {
                name = Enum.GetName(typeof(T), 0);
            }

            return (T)Enum.Parse(typeof(T), name);
        }
    }
}