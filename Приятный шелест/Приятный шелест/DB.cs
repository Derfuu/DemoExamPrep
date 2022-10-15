using System.Data.SqlClient;

namespace Приятный_шелест
{
    internal class DB
    {
        //DESKTOP-PUEBN7O
        //LAPTOP-TK7JKUOV
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-PUEBN7O; Initial Catalog=test; Integrated Security=True");

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
