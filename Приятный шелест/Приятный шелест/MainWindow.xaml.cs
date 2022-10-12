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
            string[] test1 = new string[10];
            string[] test2 = new string[10];
            string[] test3 = new string[10];
            double[] test4 = new double[10];
            // string test = "";
            string queryString = $"select top (10) [Тип агента], [Наименование агента]," +
                $" [Телефон агента], [Приоритет] from agents_b_import2$";
            SqlCommand command = new SqlCommand(queryString, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                test1[i] = reader.GetString(0);
                test2[i] = reader.GetString(1);
                test3[i] = reader.GetString(2);
                test4[i] = reader.GetDouble(3);
                i++;
            }
            reader.Close();
            // test.ItemsSource = test1;
            for (i = 0; i < 10; i++)
            {
                Grid el = new Grid();
                //list.Height = 75;
                ColumnDefinition img = new ColumnDefinition();
                ColumnDefinition descript = new ColumnDefinition();
                el.ColumnDefinitions.Add(img);
                el.ColumnDefinitions.Add(descript);

                //agentNameLabel settings
                Label agentNameLabel = new Label();
                agentNameLabel.Content = test2[i];
                //MessageBox.Show(test2[i]);
                // agentNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                // agentNameLabel.VerticalAlignment = VerticalAlignment.Top;
                // agentNameLabel.Margin = new Thickness(10);
                // agentNameLabel.Width = Double.NaN;
                Grid.SetColumn(agentNameLabel, 1);

                //create instanses for grid
                //innerGrid.Children.Add(agentLogo);
                //innerGrid.Children.Add(agentLogoBorder);
                //innerGrid.Children.Add(agentDataBorder);
                el.Children.Add(agentNameLabel);
                //innerGrid.Children.Add(agentDiscountLabel);

                //inserting ready row in grid
                RowDefinition rowDef = new RowDefinition();
                rowDef.MinHeight = 75;
                rowDef.MaxHeight = 75;
                rowDef.Name = $"row{i}";
                list.RowDefinitions.Add(rowDef);
                Grid.SetRow(el, i);
                list.Children.Add(el);
                el.UpdateLayout();
            }
            list.UpdateLayout();
            // Binding binding = new Binding();
            // binding.ElementName = "test1";
            // binding.Source = test;
            // test1.SetBinding(TextBlock.TextProperty, test[0]);
        }
    }
}
