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
            //string[] test1 = new string[10];
            string[] name = new string[10];
            double[] prod = new double[10];
            string[] phone = new string[10];
            double[] priorety = new double[10];
            // string test = "";
            string queryString = $"select top (10) [Тип агента], [Наименование агента]," +
                $"[Телефон агента], [Приоритет] from agents_b_import2$;";
            //$"select top (10) [Количество продукции] from Лист1$";
            SqlCommand command = new SqlCommand(queryString, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                //test1[i] = reader.GetString(0);
                name[i] = reader.GetString(0) + " | " +reader.GetString(1);
                phone[i] = reader.GetString(2);
                priorety[i] = reader.GetDouble(3);
                //prod[i] = reader.GetDouble(4);
                i++;
            }
            reader.Close();
            //SqlDataReader reader1 = command.ExecuteReader();
            //i = 0;
            //string queryString1 = $"select top (10) [Количество продукции] from [Лист1$]";
            //SqlCommand command1 = new SqlCommand(queryString1, db.getConnection());
            //while (reader1.Read())
            //{
            //    prod[i] = reader.GetDouble(0);
            //    i++;
            //}
            //reader1.Close();
            
            // test.ItemsSource = test1;
            SolidColorBrush bgcolor = new SolidColorBrush(Color.FromArgb(0xFF, 0xC6, 0xD7, 0xFF));
            for (i = 0; i < 10; i++)
            {
                Grid el = new Grid();
                //list.Height = 75;
                ColumnDefinition img = new ColumnDefinition();
                ColumnDefinition descript = new ColumnDefinition();
                img.Width = new GridLength(100);

                el.ColumnDefinitions.Add(img);
                el.ColumnDefinitions.Add(descript);

                //agentNameLabel settings
                Image leftSide = new Image();
                leftSide.Source = new BitmapImage(new Uri("/picture.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };;
                leftSide.Height = 75;
                leftSide.Margin = new Thickness(5,5,0,5);
                leftSide.VerticalAlignment = VerticalAlignment.Top;
                //this.image1.Source = new BitmapImage(new Uri("/Resources/00223.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
            
                //MessageBox.Show(test2[i]);
                // agentNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                // agentNameLabel.VerticalAlignment = VerticalAlignment.Top;
                // agentNameLabel.Margin = new Thickness(10);
                // agentNameLabel.Width = Double.NaN;
                Grid.SetColumn(leftSide, 0);

                //agentNameLabel settings
                Label agentNameLabel = new Label();
                agentNameLabel.Content = name[i];
                Grid.SetColumn(agentNameLabel, 1);

                //create instanses for grid
                //innerGrid.Children.Add(agentLogo);
                //innerGrid.Children.Add(agentLogoBorder);
                //innerGrid.Children.Add(agentDataBorder);
                el.Children.Add(leftSide);
                el.Children.Add(agentNameLabel);
                //innerGrid.Children.Add(agentDiscountLabel);

                Border Border1 = new Border();
                Border1.BorderThickness = new Thickness(1);
                //Border1.Background = bgcolor;
                Border1.BorderBrush = bgcolor;
                Border1.Margin = new Thickness(0, 0, 15, 15);
                //Grid.SetRow(Border1, 0);
                //inserting ready row in grid
                
                RowDefinition rowDef = new RowDefinition();
                rowDef.MinHeight = 100;
                rowDef.MaxHeight = 100;
                rowDef.Name = $"row{i}";
                list.RowDefinitions.Add(rowDef);
                Grid.SetRow(el, i);
                Grid.SetColumnSpan(el, 3);
                //Grid.SetColumn(el, 1);
                list.Children.Add(el);
                Grid.SetRow(Border1, i);
                Grid.SetColumnSpan(Border1, 3);
                //Grid.SetColumn(Border1, 1);
                list.Children.Add(Border1);
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
