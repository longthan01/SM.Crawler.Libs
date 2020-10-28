using System.IO;
using System.Xml.Serialization;

namespace SM.Libs.Utils
{
    public static class XmlUtils
    {
        public static string ToXml<T>(this T obj, string rootTag = null)
        {
            XmlSerializer xmlSerializer = null;
            if (!string.IsNullOrWhiteSpace(rootTag))
            {
                xmlSerializer = new XmlSerializer(obj.GetType(),
                    new XmlRootAttribute(rootTag));
            }
            else
            {
                xmlSerializer = new XmlSerializer(obj.GetType());
            }

            StringWriter sw = new StringWriter();
            xmlSerializer.Serialize(sw, obj);
            return sw.ToString();
        }
    }
}
