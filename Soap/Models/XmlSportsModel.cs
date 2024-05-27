using System.Xml.Serialization;

namespace Soap.Models;

[XmlRoot("XmlSports")]
public class XmlSportsModel
{
    [XmlAttribute("CreateDate")]
    public DateTime CreateDate { get; set; }
    
    [XmlElement("Sport")]
    public List<SportXmlModel> Sports { get; set; }
}