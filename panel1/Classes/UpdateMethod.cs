using Panel1.Classes;

namespace panel1.Classes
{
    public class UpdateMethod
    {
        private readonly Connection _connection;

        public UpdateMethod(Connection connection)
        {
            _connection = connection;
        }

        public void Update(string tableName, string[] fields, string[] values, string idColumnName, int id)
        {


            string setClause = string.Join(", ", Enumerable.Range(0, fields.Length).Select(i => $"{fields[i]} = '{values[i]}'"));
            string query = $"UPDATE {tableName} SET {setClause} WHERE {idColumnName} = {id}";

            _connection.ExecuteQueryWithoutResult(query);
        }
    }
}
