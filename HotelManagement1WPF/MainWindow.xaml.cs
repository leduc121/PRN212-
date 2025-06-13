using HotelManagement.BLL.Services;
using HotelManagement.DAL;
using HotelManagement.DAL.Repositories;
using HotelManagement.DAL.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement1WPF
{
    public partial class MainWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;
        private readonly ReportService _reportService;
        private readonly int _customerId;
        private readonly bool _isAdmin;

        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<RoomInformation> Rooms { get; set; }
        public ObservableCollection<BookingReservation> Bookings { get; set; }
        public ObservableCollection<BookingReservation> Reports { get; set; }

        public MainWindow(int customerId, bool isAdmin)
        {
            _customerId = customerId;
            _isAdmin = isAdmin;
            var context = new HotelManagement.DAL.FuminiHotelManagementContext();
            var repositoryFactory = new RepositoryFactory(context);
            _customerService = new CustomerService(repositoryFactory.CreateCustomerRepository());
            _roomService = new RoomService(repositoryFactory);
            _bookingService = new BookingService(repositoryFactory);
            _reportService = new ReportService(repositoryFactory);

            Customers = new ObservableCollection<Customer>(_customerService.GetAll());
            Rooms = new ObservableCollection<RoomInformation>(_roomService.GetAllRooms());
            Bookings = new ObservableCollection<BookingReservation>(_bookingService.GetBookingsByCustomerId(_customerId));
            Reports = new ObservableCollection<BookingReservation>();

            InitializeComponent();
            UpdateUIBasedOnRole();
            if (_isAdmin)
                MainTabControl.SelectedIndex = 0; // Chuyển sang tab Customers cho Admin
            else
                MainTabControl.SelectedIndex = 2; // Chuyển sang tab Bookings cho Customer
        }

        private void UpdateUIBasedOnRole()
        {
            if (_isAdmin)
            {
                CustomersTab.Visibility = Visibility.Visible;
                RoomsTab.Visibility = Visibility.Visible;
                ReportsTab.Visibility = Visibility.Visible;
                BookingsTab.Visibility = Visibility.Collapsed;
                customerDataGrid.ItemsSource = Customers;
                roomDataGrid.ItemsSource = Rooms;
                reportDataGrid.ItemsSource = Reports;
                AddCustomer.IsEnabled = true;
                UpdateCustomer.IsEnabled = true;
                DeleteCustomer.IsEnabled = true;
                AddRoom.IsEnabled = true;
                UpdateRoom.IsEnabled = true;
                DeleteRoom.IsEnabled = true;
                GenerateReportButton.IsEnabled = true;
            }
            else
            {
                CustomersTab.Visibility = Visibility.Visible;
                RoomsTab.Visibility = Visibility.Collapsed;
                ReportsTab.Visibility = Visibility.Collapsed;
                BookingsTab.Visibility = Visibility.Visible;
                customerDataGrid.ItemsSource = new ObservableCollection<Customer> { _customerService.GetById(_customerId) };
                bookingDataGrid.ItemsSource = Bookings;
                AddCustomer.IsEnabled = false;
                UpdateCustomer.IsEnabled = true;
                DeleteCustomer.IsEnabled = false;
                AddRoom.IsEnabled = false;
                UpdateRoom.IsEnabled = false;
                DeleteRoom.IsEnabled = false;
                GenerateReportButton.IsEnabled = false;
            }
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customerDataGrid.SelectedItem is Customer customer)
            {
                customerFullNameTextBox.Text = customer.CustomerFullName ?? "";
                customerEmailTextBox.Text = customer.EmailAddress ?? "";
                customerTelephoneTextBox.Text = customer.Telephone ?? "";
                customerPasswordTextBox.Text = customer.Password ?? "";
            }
        }

        private void RoomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (roomDataGrid.SelectedItem is RoomInformation room)
            {
                roomNumberTextBox.Text = room.RoomNumber ?? "";
                roomDescriptionTextBox.Text = room.RoomDetailDescription ?? "";
                roomMaxCapacityTextBox.Text = room.RoomMaxCapacity?.ToString() ?? "";
                roomTypeIdTextBox.Text = room.RoomTypeId.ToString() ?? ""; // Cast int to string
                roomStatusTextBox.Text = room.RoomStatus?.ToString() ?? "";
                roomPricePerDayTextBox.Text = room.RoomPricePerDay?.ToString() ?? "";
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin)
            {
                var customer = new Customer
                {
                    CustomerFullName = customerFullNameTextBox.Text,
                    EmailAddress = customerEmailTextBox.Text,
                    Telephone = customerTelephoneTextBox.Text,
                    Password = customerPasswordTextBox.Text,
                    CustomerStatus = 1
                };
                try
                {
                    _customerService.Add(customer);
                    Customers.Add(customer);
                    ClearCustomerFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi thêm khách hàng: {ex.Message}");
                }
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = customerDataGrid.SelectedItem as Customer;
            if (customer != null)
            {
                customer.CustomerFullName = customerFullNameTextBox.Text;
                customer.EmailAddress = customerEmailTextBox.Text;
                customer.Telephone = customerTelephoneTextBox.Text;
                customer.Password = customerPasswordTextBox.Text;
                try
                {
                    _customerService.Update(customer);
                    ClearCustomerFields();
                    customerDataGrid.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi cập nhật khách hàng: {ex.Message}");
                }
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin && customerDataGrid.SelectedItem is Customer selectedCustomer)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _customerService.Delete(selectedCustomer.CustomerId);
                        Customers.Remove(selectedCustomer);
                        ClearCustomerFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xóa khách hàng: {ex.Message}");
                    }
                }
            }
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin)
            {
                var room = new RoomInformation
                {
                    RoomNumber = roomNumberTextBox.Text,
                    RoomDetailDescription = roomDescriptionTextBox.Text,
                    RoomMaxCapacity = int.TryParse(roomMaxCapacityTextBox.Text, out int maxCapacity) ? maxCapacity : (int?)null,
                    RoomTypeId = int.TryParse(roomTypeIdTextBox.Text, out int roomTypeId) ? roomTypeId : 0, // Giả sử giá trị mặc định nếu không hợp lệ
                    RoomStatus = byte.TryParse(roomStatusTextBox.Text, out byte roomStatus) ? roomStatus : (byte?)null,
                    RoomPricePerDay = decimal.TryParse(roomPricePerDayTextBox.Text, out decimal price) ? price : (decimal?)null
                };
                try
                {
                    _roomService.AddRoom(room);
                    Rooms.Add(room);
                    ClearRoomFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi thêm phòng: {ex.Message}");
                }
            }
        }

        private void UpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin && roomDataGrid.SelectedItem is RoomInformation selectedRoom)
            {
                selectedRoom.RoomNumber = roomNumberTextBox.Text;
                selectedRoom.RoomDetailDescription = roomDescriptionTextBox.Text;
                selectedRoom.RoomMaxCapacity = int.TryParse(roomMaxCapacityTextBox.Text, out int maxCapacity) ? maxCapacity : selectedRoom.RoomMaxCapacity;
                selectedRoom.RoomTypeId = int.TryParse(roomTypeIdTextBox.Text, out int roomTypeId) ? roomTypeId : selectedRoom.RoomTypeId;
                selectedRoom.RoomStatus = byte.TryParse(roomStatusTextBox.Text, out byte roomStatus) ? roomStatus : selectedRoom.RoomStatus;
                selectedRoom.RoomPricePerDay = decimal.TryParse(roomPricePerDayTextBox.Text, out decimal price) ? price : selectedRoom.RoomPricePerDay;
                try
                {
                    _roomService.UpdateRoom(selectedRoom);
                    ClearRoomFields();
                    roomDataGrid.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi cập nhật phòng: {ex.Message}");
                }
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin && roomDataGrid.SelectedItem is RoomInformation selectedRoom)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _roomService.DeleteRoom(selectedRoom.RoomId);
                        Rooms.Remove(selectedRoom);
                        ClearRoomFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xóa phòng: {ex.Message}");
                    }
                }
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdmin)
            {
                var startDate = startDatePicker.SelectedDate ?? DateTime.Now.AddDays(-30);
                var endDate = endDatePicker.SelectedDate ?? DateTime.Now;
                var report = _reportService.GenerateReport(startDate, endDate);
                Reports.Clear();
                foreach (var item in report)
                {
                    Reports.Add(item);
                }
            }
        }

        private void ClearCustomerFields()
        {
            customerFullNameTextBox.Text = "";
            customerEmailTextBox.Text = "";
            customerTelephoneTextBox.Text = "";
            customerPasswordTextBox.Text = "";
        }

        private void ClearRoomFields()
        {
            roomNumberTextBox.Text = "";
            roomDescriptionTextBox.Text = "";
            roomMaxCapacityTextBox.Text = "";
            roomTypeIdTextBox.Text = "";
            roomStatusTextBox.Text = "";
            roomPricePerDayTextBox.Text = "";
        }
    }
}