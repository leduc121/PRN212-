using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL.Entities;

namespace HotelManagement.DAL.Repositories
{
    public interface IBookingRepository
    {
        IEnumerable<BookingReservation> GetAll();
        BookingReservation GetById(int id);
        void Add(BookingReservation booking);
        void Update(BookingReservation booking);
        void Delete(int id);
        IEnumerable<BookingReservation> GetByCustomerId(int customerId);
    }
}
