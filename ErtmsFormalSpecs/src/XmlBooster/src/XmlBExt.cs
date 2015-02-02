using System.Collections;
using System.Collections.Generic;

namespace XmlBooster
{
    public static class XmlBExt
    {
        public static void XmlBAddRange<T>(this ICollection<T> it, IEnumerable<T> collection)
        {
            foreach (T t in collection) it.Add(t);
        }

        public static void XmlBAddRange<T>(this ICollection<T> it, IEnumerable collection)
        {
            foreach (T t in collection) it.Add(t);
        }

        public static void XmlBAddRange<T>(this List<T> it, IEnumerable<T> collection)
        {
            it.AddRange(collection);
        }

        public static void XmlBAddRange(this ArrayList it, ICollection collection)
        {
            it.AddRange(collection);
        }
    }
}