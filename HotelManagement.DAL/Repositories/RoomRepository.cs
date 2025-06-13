using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DAL.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public RoomRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomInformation> GetAll()
        {
            return _context.RoomInformations.ToList();
        }

        public RoomInformation GetById(int id)
        {
            return _context.RoomInformations.Find(id);
        }

        public void Add(RoomInformation room)
        {
            _context.RoomInformations.Add(room);
            _context.SaveChanges();
        }

        public void Update(RoomInformation room)
        {
            _context.RoomInformations.Update(room);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var room = GetById(id);
            if (room != null)
            {
                _context.RoomInformations.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}
