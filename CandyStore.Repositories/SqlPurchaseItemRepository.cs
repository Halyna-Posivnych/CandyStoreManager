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
    public class SqlPurchaseItemRepository : IPurchaseItemRepository
    {
        #region Queries

        private const string spAddPurchaseItemQuery = "spAddPurchaseItem";
        private const string spspGetCurrentPurchaseItemsQuery = "spGetCurrentPurchaseItems";
        private const string spGetPurchaseItemByIdQuery = "spGetPurchaseItemById";

        #endregion

        #region Private Fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public SqlPurchaseItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region IPurchaseRepository
        
        public PurchaseItem AddPurchaseItem(object customerId, int purchaseId, int candyId, decimal amount)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spAddPurchaseItemQuery;
                    if(customerId != null)
                    {
                        command.Parameters.AddWithValue("@customerId", (int)customerId);
                    }
                    command.Parameters.AddWithValue("@purchaseId", purchaseId);
                    command.Parameters.AddWithValue("@candyId", candyId);
                    command.Parameters.AddWithValue("@amount", amount);

                    SqlParameter newId = new SqlParameter("@newId", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    command.Parameters.Add(newId);

                    SqlParameter costParam = new SqlParameter("@cost", SqlDbType.Decimal);
                    costParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(costParam);

                    command.ExecuteNonQuery();

                    PurchaseItem purchaseItem = new PurchaseItem();

                    purchaseItem.Id = (int)command.Parameters["@newId"].Value;
                    purchaseItem.PurchaseId = purchaseId;
                    purchaseItem.CandyId = candyId;
                    purchaseItem.Amount = amount;
                    purchaseItem.Cost = (decimal)command.Parameters["@cost"].Value;

                    return purchaseItem;
                }
            }
        }

        public IEnumerable<PurchaseItem> GetCurrentPurchaseItems(int purchaseId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spspGetCurrentPurchaseItemsQuery;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<PurchaseItem> currentPurchaseItems = new List<PurchaseItem>();

                        while (reader.Read())
                        {
                            PurchaseItem purchaseItem = new PurchaseItem();

                            purchaseItem.Id = (int)reader["Id"];
                            purchaseItem.PurchaseId = (int)reader["PurchaseId"];
                            purchaseItem.CandyId = (int)reader["CandyId"];
                            purchaseItem.Amount = (decimal)reader["Amount"];
                            purchaseItem.Cost = (decimal)reader["Cost"];

                            currentPurchaseItems.Add(purchaseItem);
                        }

                        return currentPurchaseItems;
                    }
                }
            }
        }

        public PurchaseItem GetPurchaseItem(int purchaseItemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetPurchaseItemByIdQuery;

                    command.Parameters.AddWithValue("@purchaseItemId", purchaseItemId);
                    PurchaseItem purchaseItem = null;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        purchaseItem = new PurchaseItem();
                        if(reader.Read())
                        {
                            purchaseItem.Id = (int)reader["Id"];
                            purchaseItem.PurchaseId = (int)reader["PurchaseId"];
                            purchaseItem.CandyId = (int)reader["CandyId"];
                            purchaseItem.Amount = (decimal)reader["Amount"];
                            purchaseItem.Cost = (decimal)reader["Cost"];
                        }
                    }
                    return purchaseItem;
                }
            }
        }

        #endregion
    }
}
