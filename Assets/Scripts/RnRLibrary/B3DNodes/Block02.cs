using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block02 : BaseGroupNode, IBoundingSphere
    {

        //TODO: Get info about 4-floats value (1,0,0,0)
        public float[] UNKNOWN;
        /// <inheritdoc />
        public Block02(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            UNKNOWN = new float[4];

            for (int i = 0; i < 4; i++)
            {
                UNKNOWN[i] = reader.ReadSingle();
            }

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}