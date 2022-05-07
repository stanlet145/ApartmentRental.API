using ApartmentRental.Core.Entities;
using ApartmentRental.Infrastructure.Context;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastructure.Repository;

public class ApartmentRepository : IApartmentRepository
{
    private readonly MainContext _mainContext;

    public ApartmentRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Apartment>> GetAll()
    {
        var apartments = await _mainContext.Apartment.ToListAsync();

        foreach (var apartment in apartments)
        {
            await _mainContext.Entry(apartment)
                .Reference(x => x.Address)
                .LoadAsync();
        }

        return apartments;
    }

    public async Task<Apartment> GetById(int id)
    {
        var apartment = await _mainContext.Apartment.SingleOrDefaultAsync(x => x.Id == id);
        if (apartment == null) throw new EntityNotFoundException();
        {
            await _mainContext.Entry(apartment).Reference(x => x.Address).LoadAsync();
            return apartment;
        }
    }

    public async Task Add(Apartment entity)
    {
        var apartmentWithExistingAddress =  await _mainContext.Apartment.SingleOrDefaultAsync(x => x.Address == entity.Address);
        if (apartmentWithExistingAddress != null) throw new EntityAlreadyExistsException();
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();

        throw new EntityAlreadyExistsException();
    }

    public async Task Update(Apartment entity)
    {
        var apartmentToUpdate = await _mainContext.Apartment.SingleOrDefaultAsync(x => x.Id == entity.Id);
        if (apartmentToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        apartmentToUpdate.Floor = entity.Floor;
        apartmentToUpdate.IsElevatorAvailable = entity.IsElevatorAvailable;
        apartmentToUpdate.Rent = entity.Rent;
        apartmentToUpdate.Area = entity.Area;
        apartmentToUpdate.AmountOfRooms = entity.AmountOfRooms;
        apartmentToUpdate.DateOfCreation = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteById(int id)
    {
        var apartmentToDelete = await _mainContext.Apartment.SingleOrDefaultAsync(x => x.Id == id);
        if (apartmentToDelete == null) throw new EntityNotFoundException();
        _mainContext.Apartment.Remove(apartmentToDelete);
        await _mainContext.SaveChangesAsync();

        throw new EntityNotFoundException();
    }

    public Task GetAddressIdOrCreateAsync(string dtoCountry, string dtoCity, string dtoZipCode, string dtoStreet,
        string dtoBuildingNumber, string dtoApartmentNumber)
    {
        throw new NotImplementedException();
    }
}