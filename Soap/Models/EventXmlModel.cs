using System.Xml.Serialization;

namespace Soap.Models;

public class EventXmlModel
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlAttribute("ID")]
    public int EventId { get; set; }
    
    [XmlAttribute("IsLive")]
    public bool IsLive { get; set; }
    
    [XmlAttribute("CategoryID")]
    public int CategoryId { get; set; }
    
    [XmlElement("Match")]
    public List<MatchXmlModel> Matches { get; set; }
}