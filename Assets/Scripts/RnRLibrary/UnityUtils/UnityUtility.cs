using System.Linq;
using RnRLibrary.B3DNodes;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    public static class UnityUtility
    {
        public static Transform CreateObject(this INode node, Transform parent, bool setPosition = true)
        {
            Transform _transform = new GameObject(node.Name).transform;

            if (setPosition)
            {
                if (node.GetType().GetInterfaces().Contains(typeof(IBoundingSphere)))
                {
                    IBoundingSphere posParams = (IBoundingSphere) node;

                    _transform.position = posParams.Position.FlipYZ();

                    // TODO: Bounding sphere support
                }
            }

            _transform.SetParent(parent, setPosition);

            return _transform;
        }
    }
}