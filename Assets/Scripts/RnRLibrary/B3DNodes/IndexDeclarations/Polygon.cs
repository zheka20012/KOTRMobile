using System.IO;
using RnRLibrary.Utility;

namespace RnRLibrary.B3DNodes
{
    /// <summary>
    /// Default Polygon declaration:
    /// only 3 vertex indices are presented
    /// </summary>
    public class Polygon : IPolygon
    {

        /// <inheritdoc />
        public void Read(BinaryReader reader)
        {
            //Skipping 2 unknown values
            //TODO: Resolve unknown values
            reader.BaseStream.Seek(8, SeekOrigin.Current);

            MaterialIndex = reader.ReadUInt32();

            Indices = new uint[reader.ReadInt32()];

            for (int i = 0; i < Indices.Length; i++)
            {
                Indices[i] = reader.ReadUInt32();
            }
        }

        /// <inheritdoc />
        public uint[] Indices { get; private set; }

        /// <inheritdoc />
        public uint MaterialIndex { get; private set; }
    }
}