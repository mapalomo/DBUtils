
namespace DBUtils.Lib.Sql
{
    using System.Data;
    using System.Data.SqlClient;

    public static class SQLHelpers
    {

        public static DataTable ExecuteSql(string query, string connectionStr = null, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                            command.Parameters.Add(param);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }

                return dt;
            }

        }
    }
}