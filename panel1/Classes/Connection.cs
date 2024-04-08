
//using Microsoft.AspNetCore.Http;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using System.Data;
//namespace Panel1.Classes
//{
//    public class Connection
//    {
//        private readonly string _ConnectionString;

//        public Connection(IConfiguration configuration)
//        {
//            _ConnectionString = configuration.GetConnectionString("dbcs");

//        }
//        //public DataTable ExecuteQueryWithResult(string query)
//        //{
//        //    using (SqlConnection connection = new SqlConnection(_ConnectionString))
//        //    {
//        //        using (SqlCommand command = new SqlCommand(query, connection))
//        //        {
//        //            connection.Open();

//        //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
//        //            {
//        //                DataTable table = new DataTable();
//        //                adapter.Fill(table);
//        //                return table;
//        //            }
//        //        }
//        //    }
//        //}
//        public DataTable ExecuteQueryWithResult(string query)
//        {
//            using (SqlConnection connection = new SqlConnection(_ConnectionString))
//            {
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    connection.Open();

//                    //using ( SqlDataReader reader= command.ExecuteReader() )
//                    //{
//                    //    DataTable.Load(reader);

//                    //}
//                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
//                    {
//                        DataTable table = new DataTable();
//                        adapter.Fill(table);
//                        return table;
//                    }
//                }
//            }
//        }
//        public void ExecuteQueryWithoutResult(string query)
//        {
//            using (SqlConnection con = new SqlConnection(_ConnectionString))
//            {
//                using (SqlCommand com = new SqlCommand(query, con))
//                {
//                    con.Open();
//                    com.ExecuteNonQuery();

//                }
//            }
//        }

//        public async Task ExecuteQueryWithoutResultAsync(string query)
//        {
//            using (SqlConnection connection = new SqlConnection(_ConnectionString))
//            {
//                await connection.OpenAsync();
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    await command.ExecuteNonQueryAsync();
//                }
//            }
//        }
//        public void ExecuteInsert(string query, Dictionary<string, object> parameters)
//        {
//            using (SqlConnection con = new SqlConnection(_ConnectionString))
//            {
//                using (SqlCommand com = new SqlCommand(query, con))
//                {
//                    con.Open();

//                    foreach (var parameter in parameters)
//                    {
//                        com.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
//                    }

//                    com.ExecuteNonQuery();
//                }
//            }
//        }


//        public void InsertDataTable(string tableName, DataTable dataTable)
//        {
//            string columns = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
//            string values = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));
//            string sqlCommandInsert = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

//            using (SqlConnection con = new SqlConnection(_ConnectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand(sqlCommandInsert, con))
//                {
//                    con.Open();
//                    foreach (DataRow row in dataTable.Rows)
//                    {
//                        cmd.Parameters.Clear();
//                        foreach (DataColumn col in dataTable.Columns)
//                            cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);
//                        int inserted = cmd.ExecuteNonQuery();
//                    }
//                }
//            }
//        }

//    }
//}
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Panel1.Classes
{
    public class Connection
    {

        private readonly string _ConnectionString;
        private readonly SqlConnection _connection;

      
        public Connection(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("dbcs");
            _connection = new SqlConnection(_ConnectionString);
        }

        public SqlConnection GetSqlConnection()
        {
            return _connection;
        }

        public DataTable ExecuteQueryWithResult(string query)
        {
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public void ExecuteQueryWithoutResult(string query)
        {
           
            using (SqlCommand com = new SqlCommand(query, _connection))
            {
                _connection.Open();
                com.ExecuteNonQuery();
            }
        }



        public void InsertStoredProcedureQuery(string SpNAme, IDictionary<string, object> parameters)
        {
            using (SqlCommand com = new SqlCommand(SpNAme, _connection))
            {
                _connection.Open();
                com.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                {
                    com.Parameters.AddWithValue(parameter.Key, parameter.Value);

                }
                //com.Parameters.AddWithValue("Department", "jnjk");
                //com.Parameters.AddWithValue("Short_Name", "jk");
                //com.Parameters.AddWithValue("Description", "h yuy");
                //com.Parameters.AddWithValue("status", "1");


                com.ExecuteNonQuery();

            }
        }
        public async Task ExecuteQueryWithoutResultAsync(string query)
        {
            await _connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public void ExecuteInsert(string query, Dictionary<string, object> parameters)
        {
            using (SqlCommand com = new SqlCommand(query, _connection))
            {
                _connection.Open();
                foreach (var parameter in parameters)
                {
                    com.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }
                com.ExecuteNonQuery();
            }
        }
        public void ExecuteInsertOrUpdate(string query, Dictionary<string, object> parameters)
        {
            using (SqlCommand com = new SqlCommand(query, _connection))
            {
                _connection.Open();

                // Add parameters
                foreach (var parameter in parameters)
                {
                    com.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }

                com.ExecuteNonQuery();
                _connection.Close();
            }
          
        }
    
        public void InsertDataTable(string tableName, DataTable dataTable)
        {
            string columns = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            string values = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));
            string sqlCommandInsert = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            using (SqlCommand cmd = new SqlCommand(sqlCommandInsert, _connection))
            {
                _connection.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    cmd.Parameters.Clear();
                    foreach (DataColumn col in dataTable.Columns)
                        cmd.Parameters.AddWithValue("@" + col.ColumnName, row[col]);
                    int inserted = cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertRowIntoTable(DataRow row, string tableName)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                // Filter out columns with null values
                var nonNullColumns = row.Table.Columns.Cast<DataColumn>()
                    .Where(c => row.IsNull(c) == false)
                    .Select(c => c.ColumnName);

                // Get column names excluding columns with null values
                string columns = string.Join(", ", nonNullColumns);

                // Get parameters excluding columns with null values
                string parameters = string.Join(", ", nonNullColumns.Select(c => "@" + c));

                string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                using (SqlCommand com = new SqlCommand(insertQuery, _connection))
                {
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        // Add parameter only if the value is not null
                        if (!row.IsNull(column))
                        {
                            com.Parameters.AddWithValue("@" + column.ColumnName, row[column]);
                        }
                    }

                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting row into table {tableName}: {ex.Message}");
            }
        }
        public string GetOldImagePathFromDatabase(int Emp_id)
        {
            string oldImagePath = null;

            try
            {
                string query = "SELECT image FROM Emp_Details WHERE Emp_id = @Emp_id";
                //Console.WriteLine($"_connection{_ConnectionString}");
                //using (SqlConnection connection = GetSqlConnection())
                //{

                    using (SqlCommand command = new SqlCommand(query, _connection))
                    {
                    _connection.Open();

                    command.Parameters.AddWithValue("@Emp_id", Emp_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldImagePath = reader["image"].ToString();
                                Console.WriteLine(oldImagePath);
                            }
                        }
                    _connection.Close();
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving old image path from database: {ex.Message}");
                
            }

            return oldImagePath;
        }
    }

}


