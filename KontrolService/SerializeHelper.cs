using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KontrolService
{
    public class SerializeHelper
    {
        public static  void SerializeProject(Project Pro,String fileName)
        {
            SerializeObject(Pro, fileName);

        }
        public static void SerializeMacro(Macro Ma, String fileName)
        {
            SerializeObject(Ma, fileName);

        }
        private static void SerializeObject(object o,String fileName)
        {

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(o.GetType());
            System.IO.TextWriter txtWriter = new System.IO.StreamWriter(fileName);
            String XML = XMLSerializer.Serialize(o, null, true);
            txtWriter.Write(XML);
            txtWriter.Close();
            txtWriter.Dispose();

            //x.Serialize(txtWriter, o);
            

        }
        public static void DeserializeProject(ref Project Pro, String fileName)
        {
           // DeSerializeObject(ref Pro, fileName);
            System.IO.StreamReader SR = new StreamReader(fileName);
            String XML = SR.ReadToEnd();
            Pro = XMLSerializer.Deserialize<Project>(XML);

        }
        public static void DeserializeMacro(ref Macro Ma, String fileName)
        {
            //DeSerializeObject(ref Ma, fileName);

            System.IO.StreamReader SR = new StreamReader(fileName);
            String XML = SR.ReadToEnd();
            Ma = XMLSerializer.Deserialize<Macro>(XML);
        }

        private static void DeSerializeObject(ref object o, String fileName)
        {
            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(o.GetType());
            //System.IO.TextWriter txtWriter = new System.IO.StreamWriter(fileName);
           // FileStream fs = new FileStream(fileName, FileMode.Open);

            System.IO.StreamReader SR = new StreamReader(fileName);
            String XML = SR.ReadToEnd();
            o = XMLSerializer.Deserialize<Project>(XML);

            //o = x.Deserialize(fs);
            //fs.Close();


           // OrderedItem i = (OrderedItem)serializer.Deserialize(fs);
           /*
            fs.Close();
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create (fileName))
            {
                o = x.Deserialize(reader);
            }
           */


        }

    }
}
