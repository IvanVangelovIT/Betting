using System.Xml.Serialization;

namespace Soap.Models;

public class SportXmlModel
{
    [XmlAttribute("Name")]
    public string Name { get; set; }    
    
    [XmlAttribute("ID")]
    public int SportId { get; set; }

    [XmlElement("Event")] public List<EventXmlModel> Events { get; set; } = new List<EventXmlModel>();
}