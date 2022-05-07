using ApartmentRental.Core.DTO;

namespace ApartmentRental.Core.Services;

public interface IApartmentService
{
    Task<IEnumerable<ApartmentBasicInformationResponseDto>> GetAllApartmentsBasinInfosAsync();
    Task AddNewApartmentToExistingLandLordAsync(ApartmentCreationRequestDto dto);
    Task<ApartmentBasicInformationResponseDto> GetTheCheapestApartmentAsync();
}