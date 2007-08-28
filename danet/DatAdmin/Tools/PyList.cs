using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
{
    public static class PyList
    {
        public static int RealIndex(Array array, int pyindex)
        {
            if (pyindex < 0) return array.Length + pyindex;
            return pyindex;
        }
        public static T[] Slice<T>(T[] array, int from, int to)
        {
            int rfrom = RealIndex(array, from);
            int rto = RealIndex(array, to);
            T[] res = new T[rto - rfrom];
            for (int i = 0; i < rto - rfrom; i++) res[i] = array[rfrom + i];
            return res;            
        }
        public static T[] SliceFrom<T>(T[] array, int from)
        {
            return Slice(array, from, array.Length);
        }
        public static T[] SliceTo<T>(T[] array, int to)
        {
            return Slice(array, 0, to);
        }
    }
}
