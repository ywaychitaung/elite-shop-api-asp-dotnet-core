using elite_shop.Models.BaseModels;

namespace elite_shop.Models.Domains;

public class Address : UuidBaseModel
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
