namespace ApartmentRental.Core.Entities;

public class Account : BaseEntity
{
    public string Name { get; set; }
    public string Surename { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool isAccountActive { get; set; }
    
    public int AddressId { get; set; }
    public Address Address { get; set; }
}