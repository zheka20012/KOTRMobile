using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class ContainerNode : BaseGroupNode, IBoundingSphere
    {

        public string SpawnNodeName;
        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();
            SpawnNodeName = reader.Read32ByteString();


            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform);

            INode nodeToSpawn = file.Find(SpawnNodeName);

            nodeToSpawn.ProcessNode(_transform, file);

            EnumTree(_transform, file);

            return _transform;
        }

        /// <inheritdoc />
        public ContainerNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }
    }
}