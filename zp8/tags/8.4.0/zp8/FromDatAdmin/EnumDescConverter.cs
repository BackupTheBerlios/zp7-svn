using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Collections.Generic;

namespace DatAdmin
{
    /// <summary>
    /// EnumConverter supporting System.ComponentModel.DescriptionAttribute
    /// </summary>
    public class EnumDescConverter : System.ComponentModel.EnumConverter
    {
        protected System.Type myVal;

        /// <summary>
        /// Gets Enum Value's Description Attribute
        /// </summary>
        /// <param name="value">The value you want the description attribute for</param>
        /// <returns>The description, if any, else it's .ToString()</returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
              (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? Texts.Get(attributes[0].Description) : value.ToString();
        }

        /// <summary>
        /// Gets the description for certaing named value in an Enumeration
        /// </summary>
        /// <param name="value">The type of the Enumeration</param>
        /// <param name="name">The name of the Enumeration value</param>
        /// <returns>The description, if any, else the passed name</returns>
        public static string GetEnumDescription(System.Type value, string name)
        {
            FieldInfo fi = value.GetField(name);
            DescriptionAttribute[] attributes =
              (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? Texts.Get(attributes[0].Description) : name;
        }

        /// <summary>
        /// Gets the value of an Enum, based on it's Description Attribute or named value
        /// </summary>
        /// <param name="value">The Enum type</param>
        /// <param name="description">The description or name of the element</param>
        /// <returns>The value, or the passed in description, if it was not found</returns>
        public static object GetEnumValue(System.Type value, string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    if (attributes[0].Description == description || Texts.Get(attributes[0].Description) == description)
                    {
                        return fi.GetValue(fi.Name);
                    }
                }
                if (fi.Name == description)
                {
                    return fi.GetValue(fi.Name);
                }
            }
            return description;
        }

        public EnumDescConverter(System.Type type)
            : base(type.GetType())
        {
            myVal = type;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is Enum && destinationType == typeof(string))
            {
                return Texts.Get(GetEnumDescription((Enum)value));
            }
            if (value is string && destinationType == typeof(string))
            {
                return GetEnumDescription(myVal, (string)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<object> vals = new List<object>();
            foreach (object val in Enum.GetValues(myVal))
            {
                vals.Add(val);
            }
            TypeConverter.StandardValuesCollection res = new StandardValuesCollection(vals);
            return res;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return GetEnumValue(myVal, (string)value);
            }
            if (value is Enum)
            {
                return GetEnumDescription((Enum)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    //public static class EnumTool
    //{
    //    public static string GetEnumDescription(Enum value)
    //    {
    //        FieldInfo fi = value.GetType().GetField(value.ToString());
    //        DescriptionAttribute[] attributes =
    //          (DescriptionAttribute[])fi.GetCustomAttributes
    //          (typeof(DescriptionAttribute), false);
    //        return Texts.Get((attributes.Length > 0) ? attributes[0].Description : value.ToString());
    //    }

    //    public static string GetEnumName(System.Type value, string description)
    //    {
    //        FieldInfo[] fis = value.GetFields();
    //        foreach (FieldInfo fi in fis)
    //        {
    //            DescriptionAttribute[] attributes =
    //              (DescriptionAttribute[])fi.GetCustomAttributes
    //              (typeof(DescriptionAttribute), false);
    //            if (attributes.Length > 0)
    //            {
    //                if (attributes[0].Description == description)
    //                {
    //                    return fi.Name;
    //                }
    //            }
    //        }
    //        return Texts.Get(description);
    //    }
    //}
}
