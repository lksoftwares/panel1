using System;

namespace panel1.Classes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMappingAttribute : Attribute
    {
        public string ColumnName { get; }

        public ColumnMappingAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
