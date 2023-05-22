using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using TicketingSolution.API.Controllers;

namespace TicketingSolution.API.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Return_Forcast_Result()
        {
            //Arrange
            ////simulate logger in controller for injecting in constructor by using Moq
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();

            var controller = new WeatherForecastController(loggerMock.Object);



            //Act
            var result = controller.Get();

            //Assert
            result.Count().ShouldBeGreaterThan(1);
            result.ShouldNotBeNull();

        }
    }
}