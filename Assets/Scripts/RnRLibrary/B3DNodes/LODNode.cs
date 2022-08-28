using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class LODNode : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public LODNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            TriggerOffset = reader.ReadVector3();
            TriggerDistance = reader.ReadSingle();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var _transform = this.CreateObject(parentTransform, false);

            EnumTree(parentTransform, file);
            
            return _transform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public Vector3 TriggerOffset { get; set; }

        public float TriggerDistance { get; set; }
    }
}