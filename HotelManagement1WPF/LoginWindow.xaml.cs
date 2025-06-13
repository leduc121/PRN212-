using HotelManagement.BLL.Services;
using HotelManagement.DAL;
using HotelManagement.DAL.Repositories;
using System.Windows;

namespace HotelManagement1WPF
{
    public partial class LoginWindow : Window
    {
        private readonly CustomerService _customerService;

        public LoginWindow()
        {
            var context = new HotelManagement.DAL.FuminiHotelManagementContext();
            var repositoryFactory = new RepositoryFactory(context);
            _customerService = new CustomerService(repositoryFactory.CreateCustomerRepository());
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            var adminEmail = "admin@FUMiniHotelSystem.com";
            var adminPassword = "@@abc123@@";

            if (email == adminEmail && password == adminPassword)
            {
                var mainWindow = new MainWindow(-1, true); // -1 cho Admin, isAdmin = true
                mainWindow.Show();
                Close();
            }
            else
            {
                var customer = _customerService.Authenticate(email, password);
                if (customer != null)
                {
                    var mainWindow = new MainWindow(customer.CustomerId, false); // isAdmin = false
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Email hoặc mật khẩu không đúng.");
                }
            }
        }
    }
}