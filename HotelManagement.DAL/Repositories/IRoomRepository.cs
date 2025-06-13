using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.DAL.Entities;

namespace HotelManagement.DAL.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<RoomInformation> GetAll();
        RoomInformation GetById(int id);
        void Add(RoomInformation room);
        void Update(RoomInformation room);
        void Delete(int id);
    }
}
