using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block08 : BaseNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block08(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            var count = reader.ReadUInt32();


        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            if (parentTransform.GetComponent<MeshFilter>() == null) // We not found mesh filter, that's wrong
            {
                Debug.LogError("Wrong b3d hierarchy! Can't find mesh vertex delaration!", parentTransform);
                return parentTransform;
            }

            return parentTransform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
        

    }
}