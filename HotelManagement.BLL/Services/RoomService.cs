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
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(RepositoryFactory repositoryFactory)
        {
            _roomRepository = repositoryFactory.CreateRoomRepository();
        }

        public IEnumerable<RoomInformation> GetAllRooms()
        {
            return _roomRepository.GetAll();
        }

        public RoomInformation GetRoomById(int id)
        {
            return _roomRepository.GetById(id);
        }

        public void AddRoom(RoomInformation room)
        {
            if (string.IsNullOrEmpty(room.RoomNumber))
                throw new ArgumentException("Room number is required.");
            _roomRepository.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            _roomRepository.Update(room);
        }

        public void DeleteRoom(int id)
        {
            _roomRepository.Delete(id);
        }
    }
}
