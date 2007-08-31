using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DAIntf
{
    public class StoredConnection<T> where T : class
    {
        public void Save(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (FileStream fw = new FileStream(file, FileMode.Create))
            {
                ser.Serialize(fw, this);
            }
        }

        public static T Load(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (FileStream fr = new FileStream(file, FileMode.Open))
            {
                T con = (T)ser.Deserialize(fr);
                return con;
            }
        }

    }
}
