namespace ApartmentRental.Core.Entities;

public class Apartment : BaseEntity
{
    public decimal Rent { get; set; }
    public int AmountOfRooms { get; set; }
    public int Area { get; set; }
    public int Floor { get; set; }
    public bool IsElevatorAvailable { get; set; }
    
    public int LandLordId { get; set; }
    public Landlord Landlord { get; set; }
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    
    public IEnumerable<Image> Images { get; set; }
}