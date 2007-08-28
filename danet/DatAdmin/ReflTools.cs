using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DatAdmin
{
    public struct MethodAttribute<T> where T : System.Attribute
    {
        public MethodInfo Method;
        public T Attribute;
        public MethodAttribute(MethodInfo method, T attribute)
        {
            Method = method;
            Attribute = attribute;
        }
    }

    public static class ReflTools
    {
        public static IEnumerable<MethodAttribute<T>> GetMethods<T>(object obj) where T:System.Attribute
        {
            foreach (MethodInfo mtd in obj.GetType().GetMethods())
            {
                T[] attr = (T[])mtd.GetCustomAttributes(typeof(T), true);
                if (attr.Length > 0)
                {
                    yield return new MethodAttribute<T>(mtd, attr[0]);
                }
            }
        }
    }
}
