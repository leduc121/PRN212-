using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelManagement.DAL.Entities;
using HotelManagement.DAL.Repositories;

namespace HotelManagement.BLL.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer Authenticate(string email, string password)
        {
            var customer = _customerRepository.GetByEmail(email);
            if (customer != null && customer.Password == password && customer.CustomerStatus == 1)
            {
                return customer;
            }
            return null;
        }

        public IEnumerable<Customer> GetAll() => _customerRepository.GetAll();
        public Customer GetById(int id) => _customerRepository.GetById(id);
        public void Add(Customer customer)
        {
            ValidateCustomer(customer);
            _customerRepository.Add(customer);
        }
        public void Update(Customer customer)
        {
            ValidateCustomer(customer);
            _customerRepository.Update(customer);
        }
        public void Delete(int id) => _customerRepository.Delete(id);

        private void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerFullName) || customer.CustomerFullName.Length > 50)
                throw new ArgumentException("Invalid customer name.");
            if (string.IsNullOrWhiteSpace(customer.EmailAddress) || customer.EmailAddress.Length > 50)
                throw new ArgumentException("Invalid email address.");
            if (!string.IsNullOrWhiteSpace(customer.Telephone) && customer.Telephone.Length > 12)
                throw new ArgumentException("Invalid telephone number.");
            if (string.IsNullOrWhiteSpace(customer.Password) || customer.Password.Length > 50)
                throw new ArgumentException("Invalid password.");
        }
    }
}
