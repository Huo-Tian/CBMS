// 位置：E:\cbms\CBMS\Database\DatabaseHelper.cs
using System;
using System.Data;
using System.Data.SqlClient;
using BookManagementSystem.Models;

namespace BookManagementSystem.Database
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString = "Data Source=library.db;Version=3;Initial Catalog=BookDB;Integrated Security=True")
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection as SqlConnection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(query, connection as SqlConnection))
                {
                    command.Parameters.AddRange(parameters);
                    var adapter = new SqlDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}