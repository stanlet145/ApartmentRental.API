using ApartmentRental.Core.Entities;
using ApartmentRental.Infrastructure.Context;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApartmentRental.Infrastructure.Repository;

public class ImageRepository : IImageRepository
{
    private readonly MainContext _mainContext;

    public ImageRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public async Task<IEnumerable<Image>> GetAll()
    {
        var images = await _mainContext.Image.ToListAsync();

        foreach (var image in images)
        {
            await _mainContext.Entry(image)
                .Reference(x => x.Apartment)
                .LoadAsync();
        }

        return images;
    }

    public async Task<Image> GetById(int id)
    {
        var image = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == id);
        if (image == null) throw new EntityNotFoundException();
        {
            await _mainContext.Entry(image).Reference(x => x.Apartment).LoadAsync();
            return image;
        }
    }

    public async Task Add(Image entity)
    {
        entity.DateOfCreation = DateTime.UtcNow;
        await _mainContext.AddAsync(entity);
        await _mainContext.SaveChangesAsync();

        throw new EntityAlreadyExistsException();
    }

    public async Task Update(Image entity)
    {
        var imageToUpdate = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == entity.Id);
        if (imageToUpdate == null)
        {
            throw new EntityNotFoundException();
        }

        imageToUpdate.Apartment = entity.Apartment;
        imageToUpdate.Data = entity.Data;
        imageToUpdate.DateOfCreation = DateTime.UtcNow;

        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteById(int id)
    {
        var imageToDelete = await _mainContext.Image.SingleOrDefaultAsync(x => x.Id == id);
        if (imageToDelete == null) throw new EntityNotFoundException();
        _mainContext.Image.Remove(imageToDelete);
        await _mainContext.SaveChangesAsync();

        throw new EntityNotFoundException();
    }
}