
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using Microsoft.AspNetCore.Http;
//using panel1.Classes;
//using Panel1.Classes;
//using Panel1.Model;

//namespace panel1.Classes
//{
//    public class InsertMethod
//    {
//        private readonly Connection _connection;

//        public InsertMethod(Connection connection)
//        {
//            _connection = connection;
//        }

//        public void InsertOrUpdateEntity(object entity, string tableName, int id, string idPropertyName = null)
//        {
//            try
//            {
//                // Dynamically get the properties of the object at runtime
//                PropertyInfo[] properties = entity.GetType().GetProperties();
//                // A dictionary named parameterDictionary is created to store parameters for the SQL query.
//                Dictionary<string, object> parameterDictionary = new Dictionary<string, object>();

//                PropertyInfo idProperty = null;
//                bool isUpdate = false;

//                if (!string.IsNullOrEmpty(idPropertyName))
//                {
//                    // Check if the entity has an ID property
//                    idProperty = properties.FirstOrDefault(p => p.Name.Equals(idPropertyName, StringComparison.OrdinalIgnoreCase));
//                    isUpdate = idProperty != null && id != 0; // If ID is provided, it's an update
//                }

//                foreach (PropertyInfo property in properties)
//                {
//                    if (property.GetCustomAttributes(typeof(SkipInsertAttribute), true).Length > 0)
//                        continue;

//                    object value = property.GetValue(entity);

//                    if (value == null)
//                        continue;
//                    //string oldimage = null;
//                    //oldimage = (string)idProperty.GetValue(entity);
//                    //if (isUpdate)
//                    //{
//                    //    if (!string.IsNullOrEmpty(oldimage))
//                    //    {
//                    //        ImagesHandler.DeleteImage(oldimage);
//                    //    }
//                    //}
//                    if (property.PropertyType == typeof(IFormFile))
//                    {
//                        // Handle file upload separately
//                        var fileProperty = (IFormFile)value;
//                        if (fileProperty != null && fileProperty.Length > 0)
//                        {
//                            string imagePath = ImagesHandler.SaveImage(fileProperty);
//                            parameterDictionary.Add(property.Name, imagePath);

//                        }
//                        continue;
//                    }

//                    if (property.Name == "password")
//                    {
//                        // Hash password if it's being inserted or updated
//                        value = HashedPassword.HashPassword((string)value);
//                    }

//                    // Check for column mapping attribute
//                    //var columnMapping = property.GetCustomAttribute<ColumnMappingAttribute>();
//                    //string columnName = columnMapping != null ? columnMapping.ColumnName : property.Name;
//                    ColumnMappingAttribute customAttribute = property.GetCustomAttribute<ColumnMappingAttribute>();
//                    string key = ((customAttribute != null) ? customAttribute.ColumnName : property.Name);
//                    // Exclude ID property from parameters for insert operation
//                    if (!isUpdate || (isUpdate && property != idProperty))
//                        parameterDictionary.Add(key, value);
//                }

//                string query;
//                if (isUpdate)
//                {
//                    // Construct update query
//                    string setClause = string.Join(", ", parameterDictionary.Keys.Select(p => $"{p} = @{p}"));
//                    query = $"UPDATE {tableName} SET {setClause} WHERE {idPropertyName} = @{idPropertyName}";
//                    parameterDictionary.Add(idPropertyName, id); // Add ID parameter for update
//                }
//                else
//                {
//                    // Construct insert query
//                    string columns = string.Join(", ", parameterDictionary.Keys);
//                    string parameters = string.Join(", ", parameterDictionary.Keys.Select(p => "@" + p));
//                    query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
//                }

//                // Execute the query with parameter values
//                _connection.ExecuteInsertOrUpdate(query, parameterDictionary);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error inserting or updating entity into table {tableName}: {ex.Message}");
//            }
//        }

//        public void InsertOrUpdateEntity(object entity, string tableName, string idPropertyName = null)
//        {
//            InsertOrUpdateEntity(entity, tableName, 0, idPropertyName);
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using panel1.Classes;
using Panel1.Classes;
using Panel1.Model;

namespace panel1.Classes
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
                // Dynamically get the properties of the object at runtime
                PropertyInfo[] properties = entity.GetType().GetProperties();
                // A dictionary named parameterDictionary is created to store parameters for the SQL query.
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
                   
                    if (property.PropertyType == typeof(IFormFile))
                    {
                        // Handle file upload separately
                        var fileProperty = (IFormFile)value;
                        if (fileProperty != null && fileProperty.Length > 0)
                        {
                           // string imagePath = ImagesHandler.SaveImage(fileProperty);
                            //parameterDictionary.Add(property.Name, imagePath);

                        }
                        continue;
                    }



                    // Check for column mapping attribute

                    ColumnMappingAttribute customAttribute = property.GetCustomAttribute<ColumnMappingAttribute>();
                    string key = ((customAttribute != null) ? customAttribute.ColumnName : property.Name);
                    if(!isUpdate || (isUpdate && property != idProperty))
                    
                   parameterDictionary.Add(key, value);
                    

                    // Exclude ID property from parameters for insert operation
                    //if (!isUpdate || (isUpdate && property != idProperty))
                    //    parameterDictionary.Add(key, value);
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










