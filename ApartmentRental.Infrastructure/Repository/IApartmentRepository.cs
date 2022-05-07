using ApartmentRental.Core.Entities;

namespace ApartmentRental.Infrastructure.Repository;

public interface IApartmentRepository : IRepository<Apartment>
{
    Task GetAddressIdOrCreateAsync(string dtoCountry, string dtoCity, string dtoZipCode, string dtoStreet, string dtoBuildingNumber, string dtoApartmentNumber);
}