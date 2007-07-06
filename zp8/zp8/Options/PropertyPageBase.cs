using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace zp8
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName;
        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    public class ModifiedPropertyDescriptor : PropertyDescriptor
    {
        PropertyDescriptor m_original;
        string m_displayName;

        public ModifiedPropertyDescriptor(PropertyDescriptor original, string displayName)
            : base(original)
        {
            m_original = original;
            m_displayName = displayName;
        }

        public override bool CanResetValue(object component)
        {
            return m_original.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return m_original.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return m_original.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return m_original.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return m_original.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            m_original.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            m_original.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return m_original.ShouldSerializeValue(component);
        }

        /*
        public override string Category
        {
            get { return m_original.Category; }
        }
        public override bool IsBrowsable
        {
            get { return m_original.IsBrowsable; }
        }
        */

        public override string DisplayName
        {
            get
            {
                if (m_displayName != null) return m_displayName;
                return m_original.DisplayName;
            }
        }
    }

    public class PropertyPageBase : ICustomTypeDescriptor
    {
        #region "Implements ICustomTypeDescriptor"

        public System.ComponentModel.AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public System.ComponentModel.TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public System.ComponentModel.EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public System.ComponentModel.PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(System.Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public System.ComponentModel.EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public System.ComponentModel.EventDescriptorCollection GetEvents(System.Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public System.ComponentModel.PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public System.ComponentModel.PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            PropertyDescriptorCollection src = TypeDescriptor.GetProperties(this, attributes, true);

            PropertyDescriptorCollection res = new PropertyDescriptorCollection(null);

            foreach (PropertyDescriptor desc in src)
            {
                string name = desc.Name;
                PropertyInfo info = this.GetType().GetProperty(name);
                string displayName = null;
                foreach (DisplayNameAttribute attr in info.GetCustomAttributes(typeof(DisplayNameAttribute), true))
                {
                    displayName = attr.DisplayName;
                }
                res.Add(new ModifiedPropertyDescriptor(desc, displayName));
            }
            return res;
        }

        public object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

    }
}
