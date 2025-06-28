using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FabricApp
{
    public static class DB
    {
        // Укажи своё подключение к SQL Server
        private static readonly string connectionString =
            "Server=DESKTOP-63KGBFH\\SQLEXPRESS;Database=FabricWarehouse;Trusted_Connection=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}