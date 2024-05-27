using System.Xml.Serialization;

public class ParseXmlElements
{
    public T ParseXml<T>(string xmlContent)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        
        using (StringReader reader = new StringReader(xmlContent))
        {
            return (T)serializer.Deserialize(reader);
        }
    }
}