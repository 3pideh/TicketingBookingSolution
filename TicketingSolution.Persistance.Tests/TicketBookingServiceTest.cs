

using Microsoft.EntityFrameworkCore;
using TicketingSolution.Domain;
using TicketingSolution.Persistence.Repositories;

namespace TicketingSolution.Persistence.Tests
{
    public class TicketBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Service()
        {
            //Arrange
            var date = new DateTime(2023, 05, 20);

            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
                .UseInMemoryDatabase("AvailableTicketTest", b => b.EnableNullChecks(false))
                .Options;

            using var context = new TicketingSolutionDbContext(dbOptions);
            context.Add(new Ticket { Id = 1, Name = "Ticket 1" });
            context.Add(new Ticket { Id = 2, Name = "Ticket 2" });
            context.Add(new Ticket { Id = 3, Name = "Ticket 3" });

            context.Add(new TicketBooking { TicketID = 1, Date = date });
            context.Add(new TicketBooking { TicketID = 2, Date = date.AddDays(-1) });

            //context.Add(new TicketBooking { TicketID = 1, Email = "T1@t1.com", Name = "T1", Family = "T1F", Date = date });
            //context.Add(new TicketBooking { TicketID = 2, Email = "T2@t2.com", Name = "T2", Family = "T2F", Date = date.AddDays(-1) });

            context.SaveChanges();

            var ticketBookingService = new TicketBookingService(context);

            //Act
            var availbleService = ticketBookingService.GetAvailableTicket(date);

            //Assert.
            Assert.Equal(1, availbleService.Count());
            //Assert.Contains(availbleService, q => q.Id == 2);
            //Assert.Contains(availbleService, q => q.Id == 3);
            //Assert.DoesNotContain(availbleService, q => q.Id == 1 );

        }


        [Fact]
        public void Should_Save_Ticket_Booking()
        {
            //Arrang
            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
             .UseInMemoryDatabase("ShouldSaveTest", b => b.EnableNullChecks(false))
             .Options;

            var ticketBooking = new TicketBooking { TicketID = 1, Date = new DateTime(2023, 05, 21) };

            //Act
            using var context = new TicketingSolutionDbContext(dbOptions);
            var ticketBookingService = new TicketBookingService(context);
            ticketBookingService.Save(ticketBooking);

            //Assert
            var bookings = context.TicketBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(ticketBooking.Date, booking.Date);


        }
    }
}