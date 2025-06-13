using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL.Entities;
using HotelManagement.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DAL.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<BookingReservation> GetAll()
        {
            return _context.BookingReservations.Include(b => b.BookingDetails).ToList();
        }

        public BookingReservation GetById(int id)
        {
            return _context.BookingReservations.Include(b => b.BookingDetails).FirstOrDefault(b => b.BookingReservationId == id);
        }

        public void Add(BookingReservation booking)
        {
            _context.BookingReservations.Add(booking);
            _context.SaveChanges();
        }

        public void Update(BookingReservation booking)
        {
            _context.BookingReservations.Update(booking);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var booking = GetById(id);
            if (booking != null)
            {
                _context.BookingReservations.Remove(booking);
                _context.SaveChanges();
            }
        }

        public IEnumerable<BookingReservation> GetByCustomerId(int customerId)
        {
            return _context.BookingReservations.Where(b => b.CustomerId == customerId).ToList();
        }
    }
}