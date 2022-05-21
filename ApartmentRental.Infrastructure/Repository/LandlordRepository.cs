using ApartmentRental.Core.Entities;
using ApartmentRental.Infrastructure.Context;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApartmentRental.Infrastructure.Repository;

public class LandlordRepository : ILandlordRepository
{
    private readonly MainContext _mainContext;
    private readonly ILogger<ILandlordRepository> _logger;

    public LandlordRepository(MainContext mainContext, ILogger<LandlordRepository> logger)
    {
        _mainContext = mainContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Landlord>> GetAll()
    {
        var landlords = await _mainContext.Landlord.ToListAsync();

        foreach (var landlord in landlords)
        {
            await _mainContext.Entry(landlord)
                .Reference(x => x.Account)
                .LoadAsync();
        }

        return landlords;
    }

    public async Task<Landlord> GetById(int id)
    {
        var landlord = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == id);
        if (landlord == null)
        {
            _logger.LogError("Cannot find landlord with provided id: {LandlordId}", id);
            throw new EntityNotFoundException();
        }

        await _mainContext.Entry(landlord).Reference(x => x.Account).LoadAsync();
        return landlord;
    }

    public async Task Add(Landlord entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();

        throw new EntityAlreadyExistsException();
    }

    public async Task Update(Landlord entity)
    {
        var landlordToUpdate = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == entity.Id);
        if (landlordToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        landlordToUpdate.Account = entity.Account;
        landlordToUpdate.Apartments = entity.Apartments;
        landlordToUpdate.AccountId = entity.AccountId;
        landlordToUpdate.DateOfCreation = DateTime.UtcNow;


        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteById(int id)
    {
        var landlordToDelete = await _mainContext.Landlord.SingleOrDefaultAsync(x => x.Id == id);
        if (landlordToDelete == null) throw new EntityNotFoundException();
        _mainContext.Landlord.Remove(landlordToDelete);
        await _mainContext.SaveChangesAsync();

        throw new EntityNotFoundException();
    }
}