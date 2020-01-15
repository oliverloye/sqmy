using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebControllers.Helpers
{
    public class CustomQueryBuilder
    {
        
        public string InsertQuery(string tableName, List<string>columnNames)
        {

  
            StringBuilder insertValues = new StringBuilder();
            var colNames = string.Join(",", columnNames);

            for (int i = 0; i < columnNames.Count; i++)
            {
                if(i < columnNames.Count - 1)
                {
                    insertValues.Append("@" + columnNames[i]+", ");
                } else
                {
                    insertValues.Append("@" + columnNames[i]);
                }
            }
                       
            string query = "INSERT INTO " + tableName + "(" + colNames + ") VALUES (" + insertValues.ToString() + ")";
            return query.ToString();
        }
    }
}