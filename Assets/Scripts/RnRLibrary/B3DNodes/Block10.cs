using System.Collections.Generic;
using System.IO;
using System.Linq;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public class Block10 : BaseGroupNode, IBoundingSphere
    {
        /// <inheritdoc />
        public Block10(NodeHeader header) : base(header)
        {
        }

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            Position = reader.ReadStruct<Vector3>();
            Radius = reader.ReadSingle();

            TriggerOffset = reader.ReadStruct<Vector3>();
            TriggerDistance = reader.ReadSingle();

            ReadChilds(reader);
        }

        /// <inheritdoc />
        public override Transform ProcessNode(Transform parentTransform)
        {
            LODGroup lodGroup;

            //check if we have one lod group
            if ((lodGroup = parentTransform.GetComponentInParent<LODGroup>()) == null)
            {
                //if none found, create new
                lodGroup = parentTransform.gameObject.AddComponent<LODGroup>();
                // By default unity generates 3 lod levels, get rid of them
                lodGroup.SetLODs(null); 
            }

            // Creating new lod struct ans pass the distance to it
            // TODO: Proper conversion between real distance (RnR) and screen fill percentage (Unity)
            LOD lod = new LOD(1F/TriggerDistance, null);

            //Getting list of LODs
            List<LOD> lods = lodGroup.GetLODs().ToList();

            //add our new lod
            lods.Add(lod);
            
            //return it back
            lodGroup.SetLODs(lods.ToArray());


            EnumTree(parentTransform);
            
            return parentTransform;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public float Radius { get; set; }

        public Vector3 TriggerOffset { get; set; }

        public float TriggerDistance { get; set; }
    }
}