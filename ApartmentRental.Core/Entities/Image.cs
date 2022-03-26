namespace ApartmentRental.Core.Entities;

public class Image : BaseEntity
{
    public byte[] Data { get; set; }
    
    public Apartment Apartment { get; set; }
}