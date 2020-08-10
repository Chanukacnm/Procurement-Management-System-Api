using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace ProcMgt_Reference_Services.Common
{
    public static class CommonGenericService<T>  where T : class
    {
        public static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                if(!typeof(T).GetProperty(prop.Name).GetGetMethod().IsVirtual)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }               
            }             
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (!typeof(T).GetProperty(prop.Name).GetGetMethod().IsVirtual)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }                        
                }                    
                table.Rows.Add(row);
            }
            return table;
        }
    }

   
}
