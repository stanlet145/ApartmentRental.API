using ApartmentRental.Core.DTO;
using ApartmentRental.Core.Services;
using ApartmentRental.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApartmentController : ControllerBase
{
    private readonly IApartmentService _apartmentService;

    public ApartmentController(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateNewApartment([FromBody] ApartmentCreationRequestDto dto)
    {
        try
        {
            await _apartmentService.AddNewApartmentToExistingLandLordAsync(dto);
        }
        catch (EntityNotFoundException)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _apartmentService.GetAllApartmentsBasinInfosAsync());
    }

    [HttpGet("GetTheCheapest")]
    public async Task<IActionResult> GetTheCheapestApartment()
    {
        var apartment = await _apartmentService.GetTheCheapestApartmentAsync();
        return Ok(apartment);
    }
}