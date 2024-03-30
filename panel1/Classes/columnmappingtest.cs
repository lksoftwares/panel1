using System;
using System.Reflection;
using LkDataConnection; 
using panel1.Model;

namespace panel1.Classes
{
    public class ColumnMappingTest
    {
        public static object ApplyColumnMapping(object entity)
        {
            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(typeof(SkipInsertAttribute), true).Length > 0)
                        continue;

                    ColumnMappingAttribute columnMapping = property.GetCustomAttribute<ColumnMappingAttribute>();
                    if (columnMapping != null)
                    {
                        string columnName = columnMapping.ColumnName;

                        object value = property.GetValue(entity);

                        PropertyInfo mappedProperty = entity.GetType().GetProperty(columnName);
                        if (mappedProperty != null)
                        {
                            mappedProperty.SetValue(entity, value);
                        }
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error applying column mapping: {ex.Message}");
            }
        }
    }
}
