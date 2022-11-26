using System.Data.SqlClient;

namespace Приятный_шелест
{
    internal class DB
    {
        //DESKTOP-GL4ISM2
        //LAPTOP-TK7JKUOV
        SqlConnection sqlConnection = new SqlConnection($@"Data Source=LAPTOP-TK7JKUOV; Initial Catalog=test; Integrated Security=True");
        //public void setConnection(string dataSource = "DESKTOP-PUEBN7O", string dataBase = "test")
        //{
        //    sqlConnection = new SqlConnection($@"Data Source={dataSource}; Initial Catalog={dataBase}; Integrated Security=True");
        //}

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
