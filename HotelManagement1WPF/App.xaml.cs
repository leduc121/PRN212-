using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using HotelManagement.BLL.Services;
using HotelManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement1WPF
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FuminiHotelManagementContext>(options =>
                options.UseSqlServer("Server=LAPTOP-13VQ2HGC\\SQLEXPRESS;uid=sa;pwd=12345;database=FUMiniHotelManagement;TrustServerCertificate=True"));
            services.AddScoped<FuminiHotelManagementContext>();
            services.AddScoped<CustomerService>();
            services.AddScoped<RoomService>();
            services.AddScoped<BookingService>();
            services.AddScoped<LoginWindow>();
            services.AddScoped<MainWindow>();
        }
    }
}