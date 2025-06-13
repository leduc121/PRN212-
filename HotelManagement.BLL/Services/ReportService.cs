using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL.Entities;
using HotelManagement.DAL.Repositories;
using HotelManagement.DAL;

namespace HotelManagement.BLL.Services
{
    public class ReportService
    {
        private readonly IBookingRepository _bookingRepository;

        public ReportService(RepositoryFactory repositoryFactory)
        {
            _bookingRepository = repositoryFactory.CreateBookingRepository();
        }

        public IEnumerable<BookingReservation> GenerateReport(DateTime startDate, DateTime endDate)
        {
            return _bookingRepository.GetAll()
                .Where(b => b.BookingDate >= DateOnly.FromDateTime(startDate) && b.BookingDate <= DateOnly.FromDateTime(endDate))
                .OrderByDescending(b => b.TotalPrice);
        }
    }
}
