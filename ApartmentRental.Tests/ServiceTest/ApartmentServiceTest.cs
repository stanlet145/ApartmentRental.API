using System.Collections.Generic;
using System.Threading.Tasks;
using ApartmentRental.Core.Entities;
using ApartmentRental.Core.Services;
using ApartmentRental.Infrastructure.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApartmentRental.Tests.ServiceTest;

public class ApartmentServiceTest
{
    [Fact]
    public async Task GetTheCheapestApartmentAsync_ShouldReturnNull_WhenApartmentsCollectionIsNull()
    {
        var sut = new ApartmentService(Mock.Of<IApartmentRepository>(), Mock.Of<ILandlordRepository>(),
            Mock.Of<IAddressService>());
        var result = await sut.GetTheCheapestApartmentAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTheCheapestApartmentAsync_ShouldReturnTheCheapestApartment()
    {
        var apartments = new List<Apartment>
        {
            new()
            {
                Address = new Address()
                {
                    City = "Gdańsk",
                    Country = "Poland",
                    Street = "Grunwaldzka",
                    ApartmentNumber = "1",
                    FlatNumber = "2",
                    PostalCode = "80-000"
                },
                Floor = 1,
                Rent = 2000,
                Area = 45,
                AmountOfRooms = 3,
                IsElevatorAvailable = true
            },
            new()
            {
                Address = new Address()
                {
                    City = "Gdynia",
                    Country = "Poland",
                    Street = "Wielkopolska",
                    ApartmentNumber = "2",
                    FlatNumber = "1",
                    PostalCode = "80-001"
                },
                Floor = 2,
                Rent = 1999,
                Area = 48,
                AmountOfRooms = 2
            }
        };
        var apartmentRepositoryMock = new Mock<IApartmentRepository>();
        apartmentRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(apartments);

        var sut = new ApartmentService(apartmentRepositoryMock.Object, Mock.Of<ILandlordRepository>(),
            Mock.Of<IAddressService>());

        var result = await sut.GetTheCheapestApartmentAsync();

        result.Should().NotBeNull();
        result.City.Should().Be("Gdynia");
        result.RentAmount.Should().Be(1999);
        result.IsElevatorInBuilding.Should().BeFalse();
    }
}