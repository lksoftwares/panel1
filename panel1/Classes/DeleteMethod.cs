using Panel1.Classes;

namespace panel1.Classes
{
    public class DeleteMethod
    {
        private Connection _connection;

        public DeleteMethod(Connection connection)
        {
            _connection = connection;
        }
        //public void delete(string tabelname,string idColumn, int id)
        //{
        //    var query = $"delete from {tabelname} where {idColumn}='{id}' ";
        //    _connection.ExecuteQueryWithoutResult(query);



        //}
        public void DeleteEntity(string tableName, int id, string idPropertyName)
        {
            try
            {
                string query = $"DELETE FROM {tableName} WHERE {idPropertyName} = @{idPropertyName}";

                // Create a parameter dictionary with the ID parameter
                Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();
                parameterDictionary.Add(idPropertyName, id);

                // Execute the delete query
                _connection.ExecuteInsertOrUpdate(query, parameterDictionary);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity from table {tableName}: {ex.Message}");
            }
        }
    }

    }
