using ApartmentRental.Core.Entities;
using ApartmentRental.Infrastructure.Context;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastructure.Repository;

public class TenantRepository : ITenantRepository
{
    private readonly MainContext _mainContext;

    public TenantRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Tenant>> GetAll()
    {
        var tenants = await _mainContext.Tenant.ToListAsync();

        foreach (var tenant in tenants)
        {
            await _mainContext.Entry(tenant)
                .Reference(x => x.Account)
                .LoadAsync();
        }

        return tenants;
    }

    public async Task<Tenant> GetById(int id)
    {
        var tenant = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == id);
        if (tenant == null) throw new EntityNotFoundException();
        {
            await _mainContext.Entry(tenant).Reference(x => x.Account).LoadAsync();
            return tenant;
        }
    }

    public async Task Add(Tenant entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();

        throw new EntityAlreadyExistsException();
    }

    public async Task Update(Tenant entity)
    {
        var tenantToUpdate = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == entity.Id);
        if (tenantToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        tenantToUpdate.Account = entity.Account;
        tenantToUpdate.AccountId = entity.AccountId;
        tenantToUpdate.Apartment = entity.Apartment;
        tenantToUpdate.DateOfCreation = DateTime.UtcNow;
        
        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteById(int id)
    {
        var tenantToDelete = await _mainContext.Tenant.SingleOrDefaultAsync(x => x.Id == id);
        if (tenantToDelete == null) throw new EntityNotFoundException();
        _mainContext.Tenant.Remove(tenantToDelete);
        await _mainContext.SaveChangesAsync();

        throw new EntityNotFoundException();
    }
}