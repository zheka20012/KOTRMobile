using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class LampNode : BaseGroupNode, IBoundingSphere
    {
        public enum LightType
        {
            Directional = 1,
            Point = 2,
            Spot = 3
        }

        /// <inheritdoc />
        public LampNode(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadVector3();
            Radius = reader.ReadSingle();

            UseLights = reader.ReadInt32() == 1;
            Y = reader.ReadInt32();
            Type = (LightType)reader.ReadInt32();
                
            Position2 = reader.ReadVector3();

            Colors = new float[12];

            for (int i = 0; i < 12; i++)
            {
                Colors[i] = reader.ReadSingle();
            }

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform, B3DFile file)
        {
            var  obj = this.CreateObject(parentTransform, false);

            obj.transform.position = Position2;

            obj.gameObject.AddComponent<Light>();

            EnumTree(obj, file);

            return obj;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public bool UseLights { get; set; }
        public int Y { get; set; }
        public LightType Type { get; set; }

        public Vector3 Position2 { get; set; }
        public float[] Colors { get; set; }
    }
}