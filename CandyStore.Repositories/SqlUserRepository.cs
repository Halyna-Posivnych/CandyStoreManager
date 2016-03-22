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
    public class SqlUserRepository : IUserRepository
    {
        #region Queries

        private const string spGetUserByLoginQuery = "spGetUserByLogin";

        #endregion

        #region Private Fields

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public SqlUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region IUserRepository

        public User GetUserByLogin(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spGetUserByLoginQuery;
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        User user = null;
                        if (reader.Read())
                        {
                            user = new User();
                            user.Id = (int)reader["Id"];
                            user.FirstName = (string)reader["FirstName"];
                            user.Surname = (string)reader["Surname"];
                            user.Login = (string)reader["Login"];
                            user.Disabled = (bool)reader["Disabled"];
                        }
                        return user;
                    }
                }
            }
        }

        #endregion
    }
}
