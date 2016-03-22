using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;

namespace CandyStore.Repositories
{
    public interface ICustomerRepository
    {
        Customer AddCustomer(string name, string surname);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomer(int customerId);

    }
}
