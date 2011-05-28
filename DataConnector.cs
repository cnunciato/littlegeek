using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace LittleGeek
{
    public class DataConnector : IDisposable
    {
        private SqlConnection sqlConn;

        public DataConnector()
        {
            string connString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            sqlConn = new SqlConnection(connString);
        }

        protected SqlConnection Connection
        {
            get { return sqlConn; }
        }

        public void Dispose()
        {
            sqlConn.Dispose();
        }

        public DataSet GetSet(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void ExecuteSql(string sql)
        {
            SqlCommand command = new SqlCommand(sql, Connection);
            Connection.Open();
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
