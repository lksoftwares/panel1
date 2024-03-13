
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace Panel1.Classes
{
    public class Connection
    {
        private readonly string _ConnectionString;

        public Connection(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("dbcs");
            
        }
        public DataTable ExecuteQueryWithResult(string query)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }
      
        public void ExecuteQueryWithoutResult(string query)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand com = new SqlCommand(query, con))
                {
                    con.Open();
                    com.ExecuteNonQuery();

                }
            }
        }

        public async Task ExecuteQueryWithoutResultAsync(string query)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command=new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}

