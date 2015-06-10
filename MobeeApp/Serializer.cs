using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MobeeApp
{
    public class Serializer
    {
        public Serializer() { }

        public string serialize(object objectToSerialize) {
            XmlSerializer xml = new XmlSerializer(objectToSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xml.Serialize(textWriter, objectToSerialize);
                return textWriter.ToString();
            }
        }

        public object deserialize(string stringToDeserialize, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (TextReader reader = new StringReader(stringToDeserialize))
            {
                return serializer.Deserialize(reader);
            }
        }
    }
}
