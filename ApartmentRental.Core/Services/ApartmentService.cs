using ApartmentRental.Core.DTO;
using ApartmentRental.Infrastructure.Exceptions;
using ApartmentRental.Infrastructure.Repository;

namespace ApartmentRental.Core.Services;

public class ApartmentService : IApartmentService
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly ILandlordRepository _landlordRepository;
    private readonly IAddressService _addressService;

    public ApartmentService(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public ApartmentService(IApartmentRepository apartmentRepository, ILandlordRepository landlordRepository,
        IAddressService addressService)
    {
        _apartmentRepository = apartmentRepository;
        _landlordRepository = landlordRepository;
        _addressService = addressService;
    }


    public async Task<IEnumerable<ApartmentBasicInformationResponseDto>> GetAllApartmentsBasinInfosAsync()
    {
        var apartments = await _apartmentRepository.GetAll();

        return apartments.Select(x => new ApartmentBasicInformationResponseDto(
            x.Rent,
            x.AmountOfRooms,
            x.Area,
            x.IsElevatorAvailable,
            x.Address.City,
            x.Address.Street
        ));
    }

    public async Task AddNewApartmentToExistingLandLordAsync(ApartmentCreationRequestDto dto)
    {
        var landlord = await _apartmentRepository.GetById(dto.LandLord);
    }

    public async Task<ApartmentBasicInformationResponseDto> GetTheCheapestApartmentAsync()
    {
        var apartments = await _apartmentRepository.GetAll();
        var cheapestOne = apartments.MinBy(x => x.Rent);
        if (cheapestOne is null) return null;
        return new ApartmentBasicInformationResponseDto(
            cheapestOne.Rent,
            cheapestOne.AmountOfRooms,
            cheapestOne.Area,
            cheapestOne.IsElevatorAvailable,
            cheapestOne.Address.City,
            cheapestOne.Address.Street);
    }
}