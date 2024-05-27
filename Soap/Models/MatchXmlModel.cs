using System.Xml.Serialization;

namespace Soap.Models;

public class MatchXmlModel : IHasId
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlAttribute("ID")]
    public int MatchId { get; set; }
    
    [XmlAttribute("StartDate")]
    public DateTime StartDate { get; set; }
    
    [XmlAttribute("MatchType")]
    public string Type { get; set; }
    
    [XmlElement("Bet")]
    public List<BetXmlModel> Bets { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        MatchXmlModel other = (MatchXmlModel)obj;

        return Name == other.Name && MatchId == other.MatchId && StartDate == other.StartDate && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, MatchId, StartDate, Type);
    }
    
    int IHasId.Id => MatchId;
}