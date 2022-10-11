using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Приятный_шелест
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        DB db = new DB();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            string[] test = new string[10];
            // string test = "";
            string queryString = $"select top (10) [Наименование агента] from agents_b_import2$";
            SqlCommand command = new SqlCommand(queryString, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                test[i] += reader.GetString(0);
                i++;
            }
            reader.Close();
        }
    }
}
