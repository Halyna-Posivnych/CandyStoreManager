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
    public class SqlPurchaseRepository : IPurchaseRepository
    {
        #region Queries

        private const string spAddPurchaseQuery = "spAddPurchase";
        private const string spGetAllPurchasesQuery = "spGetAllPurchases";
        private const string spConfirmPurchaseQuery = "spConfirmPurchase";

        #endregion

        #region Private Fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public SqlPurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region IPurchaseRepository

        public Purchase AddPurchase(object customerId, DateTime time, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spAddPurchaseQuery;
                    if(customerId != null)
                    {
                        command.Parameters.AddWithValue("@customerId", (int)customerId);
                    }
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@time", time);

                    SqlParameter newId = new SqlParameter("@newId", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;

                    command.Parameters.Add(newId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Purchase purchase = null;
                        if (reader.Read())
                        {
                            purchase = new Purchase();
                            purchase.Id = (int)reader["Id"];
                            if (customerId != null)
                            {
                                purchase.CustomerId = (int)reader["CustomerId"];
                            }
                            purchase.Cost = (decimal)reader["Cost"];
                            purchase.PurchaseTime = (DateTime)reader["PurchaseTime"];
                            purchase.UserId = (int)reader["UserId"];
                        }
                        return purchase;
                    }
                }
            }
        }

        public decimal ConfirmPurchase(int purchaseId, object customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spConfirmPurchaseQuery;
                    if(customerId != null)
                    {
                        command.Parameters.AddWithValue("@customerId", (int)customerId);
                    }
                    
                    command.Parameters.AddWithValue("@purchaseId", purchaseId);

                    SqlParameter newDiscount = new SqlParameter("@discount", SqlDbType.Decimal);
                    newDiscount.Direction = ParameterDirection.Output;

                    command.Parameters.Add(newDiscount);

                    command.ExecuteNonQuery();

                    decimal discount = (decimal)command.Parameters["@discount"].Value;
                    return discount;
                }
            }
        }

        public IEnumerable<Purchase> GetAllPurchases()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetAllPurchasesQuery;
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Purchase> allPurchases = new List<Purchase>();
                        while (reader.Read())
                        {
                            Purchase purchase = new Purchase();
                            purchase.Id = (int)reader["Id"];

                            int customerIdColumn = reader.GetOrdinal("CustomerId");
                            if (!reader.IsDBNull(customerIdColumn))
                            {
                                purchase.CustomerId = (int)reader["CustomerId"];
                            }

                            purchase.Cost = (decimal)reader["Cost"];
                            purchase.PurchaseTime = (DateTime)reader["PurchaseTime"];
                            purchase.UserId = (int)reader["UserId"];

                            allPurchases.Add(purchase);
                        }
                        return allPurchases;
                    }
                }
            }
        }

        #endregion
    }
}
