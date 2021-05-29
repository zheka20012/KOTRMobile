using System;
using System.Collections.Generic;

namespace RnRLibrary.B3DNodes
{
    public static class PolygonResolver
    {
        private static Dictionary<int, Type> PolygonTypes = new Dictionary<int, Type>()
        {
            {0, typeof(Polygon)},
            {1, typeof(Polygon)},
            {16, typeof(Polygon)},
            {17, typeof(Polygon)},
        };

        public static IPolygon ResolvePolygon(int polygonType)
        {
            if (PolygonTypes.ContainsKey(polygonType))
            {
                return Activator.CreateInstance(PolygonTypes[polygonType]) as IPolygon;
            }

            throw new NotSupportedException($"Block 35: Polygon type {polygonType} isn't supported yet!");
        }
    }
}