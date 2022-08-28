using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RnRLibrary.Utility
{
    public static class MeshUtility
    {
        public static Vector3 FlipYZ(this Vector3 src)
        {
            return new Vector3(src.x, src.z, src.y);
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

        public static List<int> Triangulate(int[] faces)
        {
            List<int> tris = new List<int>();

            for (int i = 0; i < faces.Length-2; i++)
            {
                tris.Add(faces[i]);
                tris.Add(faces[i + 1]);
                tris.Add(faces[i + 2]);
            }

            return tris;
        }

        public static List<Vector3> Extrude(Vector3 a, Vector3 b, Vector3 c)
        {
            List<Vector3> newVerts = new List<Vector3>();

            Vector3 d = b - a;
            Vector3 e = c - a;

            Vector3 perp = Vector3.Cross(d, e);
            perp = perp * 0.001f;
            if (perp == new Vector3(0, 0, 0))
            {
                perp = new Vector3(0, -1, 0);
            }


            List<Vector3> verts = new List<Vector3>();
            List<int> tria = new List<int>();

            newVerts.Add(a + perp);
            newVerts.Add(b + perp);
            newVerts.Add(c + perp);


            return newVerts;
        }
    }
}