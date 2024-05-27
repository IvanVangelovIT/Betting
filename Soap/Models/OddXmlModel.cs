using System.Xml.Serialization;

namespace Soap.Models;

public class OddXmlModel : IHasId
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    
    [XmlAttribute("ID")]
    public int OddId { get; set; }
    
    [XmlAttribute("Value")]
    public decimal Value { get; set; }

    [XmlAttribute("SpecialBetValue")]
    public decimal SpecialBetValue { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        OddXmlModel other = (OddXmlModel)obj;

        return Name == other.Name && OddId == other.OddId && Value == other.Value && SpecialBetValue == other.SpecialBetValue;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, OddId, Value, SpecialBetValue);
    }
    
    int IHasId.Id => OddId;
}