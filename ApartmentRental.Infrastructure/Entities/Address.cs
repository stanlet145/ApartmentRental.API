using System.Diagnostics.CodeAnalysis;

namespace ApartmentRental.Core.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; } = null!;
    public string? ApartmentNumber { get; set; }
    public string? FlatNumber { get; set; }
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}