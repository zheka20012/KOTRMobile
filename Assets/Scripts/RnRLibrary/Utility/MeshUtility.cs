using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RnRLibrary.Utility
{
    public static class MeshUtility
    {
        public static Vector3 FlipYZ(this Vector3 src)
        {
            return new Vector3(src.x, src.x, src.y);
        }

        public static Vector3[] FlipYZ(this Vector3[] src)
        {
            Vector3[] outValues = new Vector3[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                outValues[i] = src[i].FlipYZ();
            }

            return outValues;
        }

        public static Vector3[] FlipYZ(this ICollection<Vector3> src)
        {
            Vector3[] outValues = new Vector3[src.Count];

            int i = 0;

            foreach (var item in src)
            {
                outValues[i] = item.FlipYZ();
                i++;
            }

            return outValues;
        }
    }
}