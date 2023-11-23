using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;
using MockQueryable.Moq;
using OfficesAPI.Controllers;
using OfficesAPI.Models;
using OfficesAPI.Services;
using Moq;

namespace UnitTests
{
    public class OfficeControllerTest
    {
        [Fact]
        public async Task GetById_WhenCalled_ReturnsOffice()
        {
            //Arrange
            var mock = TestDataHelper.GetFakeOfficesList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(1)).ReturnsAsync(
                TestDataHelper.GetFakeOfficesList().Find(e => e.Id == 1));

            var officeContextMock = new Mock<OfficesDbContext>();
            officeContextMock.Setup(x => x.Offices)
                .Returns(mock.Object);

            var officeService = new OfficeService(officeContextMock.Object);
            var controller = new OfficeController(officeService);

            //Act
            var result = await controller.GetById(1);

            //Assert
            var okResult = result as OkObjectResult;
            var office = okResult.Value as Office;
            Assert.NotNull(office);
            Assert.Equal(1, office.Id);
        }

        [Fact]
        public async void GetAll_WhenCalled_ReturnsOfficesList()
        {
            //arrange
            var mock = TestDataHelper.GetFakeOfficesList().BuildMock().BuildMockDbSet();
            var officeContextMock = new Mock<OfficesDbContext>();
            officeContextMock.Setup(x => x.Offices)
                .ReturnsDbSet(mock.Object);

            var officeService = new OfficeService(officeContextMock.Object);
            var controller = new OfficeController(officeService);

            //act
            var result = await controller.GetAll();

            //assert
            var okResult = result as OkObjectResult;
            var offices = okResult.Value as List<Office>;
            Assert.NotNull(offices);
            Assert.Equal(2, offices.Count);
        }
    }
}