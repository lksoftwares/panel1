//using Microsoft.Data.SqlClient;
//using Panel1.Classes;

//namespace panel1.Classes
//{
//    public class InsertMethod
//    {
//        private readonly Connection _connection;
//        public InsertMethod(Connection connection)
//        {
//            _connection = connection;

//        }

//        public void Insert(string tableName, string[] fields, string[] values)
//        {


//            string fieldNames = string.Join(", ", fields);
//            string fieldValues = string.Join(", ", values.Select(value => $"'{value}'"));

//            string query = $"INSERT INTO {tableName} ({fieldNames}) VALUES ({fieldValues})";

//            _connection.ExecuteQueryWithoutResult(query);
//        }
//    }
//}

//-------------------------------------------using Datatbel conversion----------------------------------------------

//using Microsoft.Data.SqlClient;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Data;
//using System.Reflection;
//using panel1.Classes;

//namespace Panel1.Classes
//{
//    public class InsertMethod
//    {
//        private readonly Connection _connection;

//        public InsertMethod(Connection connection)
//        {
//            _connection = connection;
//        }

//        public void InsertEntityIntoTable(object entity, string tableName)
//        {
//            try
//            {
//                DataTable dataTable = ConvertToDataTable(entity);

//                foreach (DataRow row in dataTable.Rows)
//                {

//                    _connection.InsertRowIntoTable(row, tableName);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error inserting entity into table {tableName}: {ex.Message}");
//            }
//        }

//        private DataTable ConvertToDataTable(object entity)
//        {
//            DataTable dataTable = new DataTable();

//            // Get properties of the entity
//            PropertyInfo[] properties = entity.GetType().GetProperties();

//            // Add columns to the DataTable
//            foreach (PropertyInfo property in properties)
//            {
//                // Check if the property type is nullable
//                Type dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

//                // Add column to the DataTable
//                dataTable.Columns.Add(property.Name, dataType);
//            }

//            // Add row to the DataTable
//            DataRow row = dataTable.NewRow();
//            foreach (PropertyInfo property in properties)
//            {
//                // Get the value of the property
//                object value = property.GetValue(entity);

//                // Check if the property type is nullable
//                Type dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

//                // Convert null value to DBNull.Value for nullable types
//                if (value == null && Nullable.GetUnderlyingType(property.PropertyType) != null)
//                {
//                    row[property.Name] = DBNull.Value;
//                }
//                else
//                {
//                    row[property.Name] = value;
//                }
//            }
//            dataTable.Rows.Add(row);

//            return dataTable;
//        }

//    }
//}





































//----------------------------Retrieve properties of the model dynamically------------------------

//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using panel1.Classes;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace Panel1.Classes
//{
//    public class InsertMethod
//    {
//        private readonly Connection _connection;

//        public InsertMethod(Connection connection)
//        {
//            _connection = connection;
//        }
//        public void InsertEntityIntoTable(object entity, string tableName)
//        {
//            try
//            {
//                // Retrieve properties of the model dynamically
//                PropertyInfo[] properties = entity.GetType().GetProperties();

//                // Prepare column names and values dynamically
//                Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();

//                foreach (PropertyInfo property in properties)
//                {
//                    // Skip properties with specific attributes if needed
//                    if (property.GetCustomAttributes(typeof(SkipInsertAttribute), true).Length > 0)
//                        continue;

//                    object value = property.GetValue(entity);

//                    if (value == null)
//                        continue;

//                    parameterDictionary.Add(property.Name, value);
//                }

//                // Construct SQL query dynamically
//                string columns = string.Join(", ", parameterDictionary.Keys);
//                string parameters = string.Join(", ", parameterDictionary.Keys.Select(p => "@" + p));

//                string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

//                // Execute the query with parameter values
//                _connection.ExecuteInsert(insertQuery, parameterDictionary);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error inserting entity into table {tableName}: {ex.Message}");
//            }
//        }

//    }
//}






//-------------------------------InsertMethodAndUpdate---------------------------------------------

using panel1.Classes;
using Panel1.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Panel1.Classes
{
    public class InsertMethod
    {
        private readonly Connection _connection;

        public InsertMethod(Connection connection)
        {
            _connection = connection;
        }
        public void InsertOrUpdateEntity(object entity, string tableName, int id, string idPropertyName = null)
        {
            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();

                Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();

                PropertyInfo idProperty = null;
                bool isUpdate = false;

                if (!string.IsNullOrEmpty(idPropertyName))
                {
                    // Check if the entity has an ID property
                    idProperty = properties.FirstOrDefault(p => p.Name.Equals(idPropertyName, StringComparison.OrdinalIgnoreCase));
                    isUpdate = idProperty != null && id != 0; // If ID is provided, it's an update
                }

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(typeof(SkipInsertAttribute), true).Length > 0)
                        continue;

                    object value = property.GetValue(entity);

                    if (value == null)
                        continue;

                    // Exclude ID property from parameters for insert operation
                    if (!isUpdate || (isUpdate && property != idProperty))
                        parameterDictionary.Add(property.Name, value);
                }

                string query;
                if (isUpdate)
                {
                    // Construct update query
                    string setClause = string.Join(", ", parameterDictionary.Keys.Select(p => $"{p} = @{p}"));
                    query = $"UPDATE {tableName} SET {setClause} WHERE {idPropertyName} = @{idPropertyName}";
                    parameterDictionary.Add(idPropertyName, id); // Add ID parameter for update
                }
                else
                {
                    // Construct insert query
                    string columns = string.Join(", ", parameterDictionary.Keys);
                    string parameters = string.Join(", ", parameterDictionary.Keys.Select(p => "@" + p));
                    query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
                }

                // Execute the query with parameter values
                _connection.ExecuteInsertOrUpdate(query, parameterDictionary);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting or updating entity into table {tableName}: {ex.Message}");
            }
        }
        public void InsertOrUpdateEntity(object entity, string tableName, string idPropertyName = null)
        {
            InsertOrUpdateEntity(entity, tableName, 0, idPropertyName); 
        }
    }
}

