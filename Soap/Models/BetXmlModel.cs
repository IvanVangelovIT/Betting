using System.Xml.Serialization;

namespace Soap.Models;

public class BetXmlModel : IHasId
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlAttribute("ID")]
    public int BetId { get; set; }
    
    [XmlAttribute("IsLive")]
    public bool IsLive { get; set; }
    
    [XmlElement("Odd")]
    public List<OddXmlModel> Odds { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        BetXmlModel other = (BetXmlModel)obj;

        return Name == other.Name && BetId == other.BetId && IsLive == other.IsLive;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, BetId, IsLive);
    }

    int IHasId.Id => BetId;
}