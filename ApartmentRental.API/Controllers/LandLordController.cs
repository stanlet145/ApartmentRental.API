using ApartmentRental.Core.DTO;
using ApartmentRental.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LandLordController : ControllerBase
{
    private readonly ILandLordService _landLordService;

    public LandLordController(ILandLordService lordService)
    {
        _landLordService = lordService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateNewLandLordAccount([FromBody] LandlordCreationRequestDto dto)
    {
        await _landLordService.CreateNewLandLordAccountAsync(dto);
        return NoContent();
    }
}