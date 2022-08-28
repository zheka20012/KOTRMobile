using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block25 : BaseNode
    {
        /// <inheritdoc />
        public Block25(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            UNKNOWN = reader.ReadInt32();
            UNKNOWN1 = reader.ReadInt32();
            UNKNOWN2 = reader.ReadInt32();

            SoundName = reader.Read32ByteString();

            UNKNOWN3 = reader.ReadInt32();
            UNKNOWN4 = reader.ReadInt32();

            UNKNOWN5 = reader.ReadSingle();
            UNKNOWN6 = reader.ReadSingle();
            UNKNOWN7 = reader.ReadSingle();
            UNKNOWN8 = reader.ReadSingle();
            UNKNOWN9 = reader.ReadSingle();
            UNKNOWN10 = reader.ReadSingle();
            UNKNOWN11 = reader.ReadSingle();
            UNKNOWN12 = reader.ReadSingle();
            UNKNOWN13 = reader.ReadSingle();
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            throw new System.NotImplementedException();
        }

        public int UNKNOWN { get; set; }
        public int UNKNOWN1 { get; set; }
        public int UNKNOWN2 { get; set; }

        public string SoundName { get; set; }

        public int UNKNOWN3 { get; set; }
        public int UNKNOWN4 { get; set; }

        public float UNKNOWN5 { get; set; }
        public float UNKNOWN6 { get; set; }
        public float UNKNOWN7 { get; set; }
        public float UNKNOWN8 { get; set; }
        public float UNKNOWN9 { get; set; }
        public float UNKNOWN10 { get; set; }
        public float UNKNOWN11 { get; set; }
        public float UNKNOWN12 { get; set; }
        public float UNKNOWN13 { get; set; }
    }
}