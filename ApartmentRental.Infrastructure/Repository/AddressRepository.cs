using ApartmentRental.Core.Entities;
using ApartmentRental.Infrastructure.Context;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastructure.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly MainContext _mainContext;

    public AddressRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Address>> GetAll()
    {
        var addresses = await _mainContext.Addresses.ToListAsync();

        foreach (var address in addresses)
        {
            await _mainContext.Entry(address)
                .Reference(x => x.FlatNumber)
                .LoadAsync();
        }

        return addresses;
    }

    public async Task<Address> GetById(int id)
    {
        var image = await _mainContext.Addresses.SingleOrDefaultAsync(x => x.Id == id);
        if (image == null) throw new EntityNotFoundException();
        {
            await _mainContext.Entry(image).Reference(x => x.FlatNumber).LoadAsync();
            return image;
        }
    }

    public async Task Add(Address entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();

        throw new EntityAlreadyExistsException();
    }

    public async Task Update(Address entity)
    {
        var addressToUpdate = await _mainContext.Addresses.SingleOrDefaultAsync(x => x.Id == entity.Id);
        if (addressToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        addressToUpdate.City = entity.City;
        addressToUpdate.Country = entity.Country;
        addressToUpdate.Street = entity.Street;
        addressToUpdate.ApartmentNumber = entity.ApartmentNumber;
        addressToUpdate.FlatNumber = entity.FlatNumber;
        addressToUpdate.PostalCode = entity.PostalCode;
        addressToUpdate.DateOfCreation = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteById(int id)
    {
        var addressToDelete = await _mainContext.Addresses.SingleOrDefaultAsync(x => x.Id == id);
        if (addressToDelete == null) throw new EntityNotFoundException();
        _mainContext.Addresses.Remove(addressToDelete);
        await _mainContext.SaveChangesAsync();

        throw new EntityNotFoundException();
    }

    public async Task<int> GetAddressesIdByItsAttributesAsync(string country, string city, string zipCode,
        string street,
        string buildingNumber, string apartmentNumber)
    {
        var address = await _mainContext.Addresses.FirstOrDefaultAsync(x =>
            x.Country == country && x.City == city && x.PostalCode == zipCode && x.Street == street && x.FlatNumber ==
            buildingNumber && x.ApartmentNumber == apartmentNumber);
        return address?.Id ?? 0;
    }
}