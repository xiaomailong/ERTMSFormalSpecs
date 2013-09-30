using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

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
    }
}
