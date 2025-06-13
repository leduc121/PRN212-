using HotelManagement.DAL.Repositories;

namespace HotelManagement.DAL
{
    public class RepositoryFactory
    {
        private readonly FuminiHotelManagementContext _context;

        public RepositoryFactory(FuminiHotelManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ICustomerRepository CreateCustomerRepository()
        {
            return new CustomerRepository(_context);
        }

        public IRoomRepository CreateRoomRepository()
        {
            return new RoomRepository(_context);
        }

        public IBookingRepository CreateBookingRepository()
        {
            return new BookingRepository(_context);
        }
    }
}