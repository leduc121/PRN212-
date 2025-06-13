using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using HotelManagement.DAL.Repositories;

namespace HotelManagement.BLL.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(RepositoryFactory repositoryFactory)
        {
            _bookingRepository = repositoryFactory.CreateBookingRepository();
        }

        public IEnumerable<BookingReservation> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }

        public IEnumerable<BookingReservation> GetBookingsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _bookingRepository.GetAll()
                .Where(b => b.BookingDate >= DateOnly.FromDateTime(startDate) && b.BookingDate <= DateOnly.FromDateTime(endDate))
                .OrderByDescending(b => b.TotalPrice);
        }

        public void AddBooking(BookingReservation booking)
        {
            _bookingRepository.Add(booking);
        }

        public IEnumerable<BookingReservation> GetBookingsByCustomerId(int customerId)
        {
            return _bookingRepository.GetByCustomerId(customerId);
        }
    }
}
