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

            _transform.SetParent(parent, true);

            return _transform;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T: Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            return transform.GetComponent<T>() == null ? transform.gameObject.AddComponent<T>() : transform.GetComponent<T>();
        }
    }
}