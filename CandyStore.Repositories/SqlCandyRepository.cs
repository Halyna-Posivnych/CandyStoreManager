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
    public class SqlCandyRepository : ICandyRepository
    {
        #region Queries

        private const string spAddNewCandyQuery = "spAddNewCandy";
        private const string spChangePromotionyQuery = "spChangePromotion";
        private const string spDeleteCandyByIdQuery = "spDeleteCandyById";
        private const string spGetAllCandiesQuery = "spGetAllCandies";
        private const string spGetCandyByIdQuery = "spGetCandyById";
        private const string spReplenishSuppliesQury = "spReplenishSupplies";
        private const string selectCandySupplyQury = "Select SupplyQty FROM tblCandy WHERE Id = {id}";

        #endregion

        #region Private Fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public SqlCandyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region IUserRepository

        public IEnumerable<Candy> GetAvailableCandies()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetAllCandiesQuery;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Candy> allAvailableCandies = new List<Candy>();
                        while (reader.Read())
                        {
                            Candy candy = new Candy();
                            candy.Id = (int)reader["Id"];
                            candy.Name = (string)reader["Name"];
                            candy.Manufacturer = (string)reader["Manufacturer"];
                            candy.Price = (decimal)reader["Price"];
                            candy.SupplyQty = (decimal)reader["SupplyQty"];
                            candy.Promotion = (decimal)reader["Promotion"];
                            candy.Deleted = (bool)reader["Deleted"];

                            allAvailableCandies.Add(candy);
                        }
                        return allAvailableCandies;
                    }

                }
            }
        }

        public Candy AddNewCandy(string name, string manufacturer, decimal price, decimal supplyQty)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spAddNewCandyQuery;
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@manufacturer", manufacturer);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@supply", supplyQty);

                    SqlParameter newId = new SqlParameter("@newId", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;

                    command.Parameters.Add(newId);

                    command.ExecuteNonQuery();

                    Candy candy = new Candy();
                    candy.Id = (int)command.Parameters["@newId"].Value;
                    candy.Name = name;
                    candy.Manufacturer = manufacturer;
                    candy.Price = price;
                    candy.SupplyQty = supplyQty;
                    candy.Promotion = 0;
                    candy.Deleted = false;

                    return candy;
                }
            }
        }

        public void ChangePromotion(int candyId, decimal newPromotion)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spChangePromotionyQuery;
                    command.Parameters.AddWithValue("@candyId", candyId);
                    command.Parameters.AddWithValue("@promotion", newPromotion);
                    
                    command.ExecuteNonQuery();
                }
            }

        }

        public void DeleteCandy(int candyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spDeleteCandyByIdQuery;
                    command.Parameters.AddWithValue("@candyId", candyId);

                    command.ExecuteNonQuery();
                }
            }
        }


        public Candy GetCandy(int candyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetCandyByIdQuery;
                    command.Parameters.AddWithValue("@candyId", candyId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Candy candy = null;
                        if (reader.Read())
                        {
                            candy = new Candy();
                            candy.Id = (int)reader["Id"];
                            candy.Name = (string)reader["Name"];
                            candy.Manufacturer = (string)reader["Manufacturer"];
                            candy.Price = (decimal)reader["Price"];
                            candy.SupplyQty = (decimal)reader["SupplyQty"];
                            candy.Promotion = (decimal)reader["Promotion"];
                            candy.Deleted = (bool)reader["Deleted"];
                            
                        }
                        return candy;
                    }
                }
            }
        }
        public bool CandyIsAvailable(int candyId, decimal amount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"Select SupplyQty FROM tblCandy WHERE Id = {candyId}";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool isAvailable = false;
                        if (reader.Read())
                        {
                            decimal availableAmount = (decimal)reader["SupplyQty"];
                            isAvailable = (amount <= availableAmount);
                        }
                        return isAvailable;
                    }
                }
            }

        }

        public void ReplenishCandySupply(int candyId, decimal supplyQty)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spReplenishSuppliesQury;
                    command.Parameters.AddWithValue("@candyId", candyId);
                    command.Parameters.AddWithValue("@supply", supplyQty);

                    command.ExecuteNonQuery();
                }
            }

        }
        #endregion
    }
}
