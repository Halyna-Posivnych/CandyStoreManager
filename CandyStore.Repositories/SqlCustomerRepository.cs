using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CandyStore.Repositories
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        #region Queries

        private const string spAddCustomerQuery = "spAddCustomer";
        private const string spGetAllCustomersQuery = "spGetAllCustomers";
        private const string spGetCustomerByIdQuery = "spGetCustomerById";

        #endregion

        #region Private Fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public SqlCustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region ICustomerRepository

        public Customer AddCustomer(string name, string surname)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spAddCustomerQuery;
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@surname", surname);

                    SqlParameter newId = new SqlParameter("@newId", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;

                    command.Parameters.Add(newId);

                    command.ExecuteNonQuery();

                    Customer customer = new Customer();
                    customer.Id = (int)command.Parameters["@newId"].Value;
                    customer.FirstName = name;
                    customer.Surname = surname;
                    customer.Discount = 0;

                    return customer;
                }
            }
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetCustomerByIdQuery;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Customer> allCustomers = new List<Customer>();
                        while (reader.Read())
                        {
                            Customer customer = new Customer();
                            customer.Id = (int)reader["Id"];
                            customer.FirstName = (string)reader["FirstName"];
                            customer.Surname = (string)reader["Surname"];
                            customer.Discount = (decimal)reader["Discount"];
                            allCustomers.Add(customer);
                        }
                        return allCustomers;
                    }
                }
            }
        }

        public Customer GetCustomer(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetCustomerByIdQuery;
                    command.Parameters.AddWithValue("@customerId", customerId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Customer customer = null;
                        if (reader.Read())
                        {
                            customer = new Customer();
                            customer.Id = (int)reader["Id"];
                            customer.FirstName = (string)reader["FirstName"];
                            customer.Surname = (string)reader["Surname"];
                            customer.Discount = (decimal)reader["Discount"];
                        }
                        return customer;
                    }
                }
            }
        }

        #endregion
    }
}
