using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoCore.Util
{
    public class DAL
    {
        private string connectionStr = @"Data Source=DESKTOP-GISUDGL\SQLEXPRESS;Initial Catalog=cursoCore;Integrated Security=True";
        private SqlConnection connection;

        public DAL()
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();
        }


        // Executa apenas SELECTs
        public DataTable SqlSelect(string sql)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(table);

            return table;
        }

        // Executa INSERTs, UPDATEs e DELETEs
        public void SqlExecute(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
        }
    }
}