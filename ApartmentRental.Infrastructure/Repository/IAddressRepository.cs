using ApartmentRental.Core.Entities;

namespace ApartmentRental.Infrastructure.Repository;

public interface IAddressRepository : IRepository<Address>
{
    Task<int> GetAddressesIdByItsAttributesAsync(string country, string city, string zipCode, string street,
        string buildingNumber, string apartmentNumber);
}