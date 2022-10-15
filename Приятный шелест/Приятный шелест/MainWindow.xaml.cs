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
    public partial class MainWindow : Window
    {
        DB db = new DB();
        public MainWindow()
        {
            InitializeComponent();
        }
        int Page = 0;
        int Paginator = 10;
        int YearsRange = 1;
        private void Window_Initialized(object sender, EventArgs e)
        {
            //string[] test1 = new string[10];
            string[] name = new string[10];
            int[] prod = {0,0,0,0,0,0,0,0,0,0};
            int[] prodID = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
            string[] phone = new string[10];
            int[] priorety = new int[10];
            string queryString = "select [AgentType].Title, [Agent].[Title], [Phone], [Priority]" +
            $"from[Agent] INNER JOIN[AgentType] ON [Agent].[AgentTypeID] = [AgentType].[ID]  where Agent.ID between {Paginator - 10} and {Paginator}";
            SqlCommand command = new SqlCommand(queryString, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                name[i] = reader.GetString(0) + " | " + reader.GetString(1);
                phone[i] = reader.GetString(2);
                priorety[i] = reader.GetInt32(3);
                i++;
            }
            reader.Close();
            command = new SqlCommand("select [ProductSale].AgentID, [ProductSale].ProductCount, [ProductSale].SaleDate from[Agent]"+
            $"INNER JOIN[ProductSale] ON[Agent].ID = [ProductSale].AgentID where Agent.ID between {Paginator - 10} and {Paginator}" +
            $"and  DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange} ORDER BY AgentID", db.getConnection());
            db.openConnection();
            reader = command.ExecuteReader();
            i = 0;
            while (reader.Read())
            {
                prodID[i] = reader.GetInt32(0);
                prod[i] = reader.GetInt32(1);
                i++;
            }
            reader.Close();
            int idProdBack = -2;
            int ContProdFirst = -1;
            int smallFont = 12;
            int bigFont = 15;
            SolidColorBrush bgcolor = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            for (i = 0; i < 10; i++)
            {
                Grid el = new Grid();
                ColumnDefinition img = new ColumnDefinition();
                ColumnDefinition descript = new ColumnDefinition();
                ColumnDefinition procent = new ColumnDefinition();
                img.Width = new GridLength(110);
                //descript.Width = new GridLength();
                procent.Width = new GridLength(100);

                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                RowDefinition row3 = new RowDefinition();
                RowDefinition row4 = new RowDefinition();
                RowDefinition row5 = new RowDefinition();
                row1.Height = new GridLength(25);
                row2.Height = new GridLength(25);
                row3.Height = new GridLength(25);
                row4.Height = new GridLength(25);
                row5.Height = new GridLength(25);
                el.RowDefinitions.Add(row1);
                el.RowDefinitions.Add(row2);
                el.RowDefinitions.Add(row3);
                el.RowDefinitions.Add(row4);
                el.RowDefinitions.Add(row5);

                el.ColumnDefinitions.Add(img);
                el.ColumnDefinitions.Add(descript);

                //agentNameLabel settings
                Image leftSide = new Image();
                leftSide.Source = new BitmapImage(new Uri("/picture.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache};
                Grid.SetRowSpan(leftSide, 4);
                leftSide.Height = 100;
                leftSide.Width = 100;
                leftSide.Margin = new Thickness(5);
                leftSide.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetColumn(leftSide, 0);

                //agentNameLabel settings
                Label agentNameLabel = new Label();
                agentNameLabel.Content = name[i];
                agentNameLabel.Margin = new Thickness(0);
                agentNameLabel.FontSize = bigFont;
                Grid.SetRow(agentNameLabel, 0);
                Grid.SetColumn(agentNameLabel, 1);

                for (int j = 0; j < 10; j++)
                {
                    if (prodID[j] == Paginator - 9 + i)
                    {
                        if (idProdBack == prodID[j])
                        {
                            prod[ContProdFirst] = prod[ContProdFirst] + prod[j];
                            prodID[j] = -1;
                            prod[j] = -1;
                        }
                        else
                        {
                            ContProdFirst = j;
                        }
                        idProdBack = prodID[j];
                    }
                }
                //prod
                Label sell = new Label();
                sell.Content = "0 Продаж за год";
                for (int j = 0; j < 10; j++)
                {
                    if (prodID[j] == Paginator - 9 + i && prodID[j] != -1)
                    {
                        sell.Content = prod[j] + " Продаж за год";
                        break;
                    }
                }
                agentNameLabel.FontSize = smallFont;
                Grid.SetRow(sell, 1);
                Grid.SetColumn(sell, 1);

                //phone 
                phone[i] = phone[i].Replace(" ", "");
                Label phoneLabel = new Label();
                phoneLabel.Content = phone[i];
                phoneLabel.FontSize = smallFont;
                Grid.SetRow(phoneLabel, 2);
                Grid.SetColumn(phoneLabel, 1);


                //priorety
                Label prioretyLabel = new Label();
                prioretyLabel.Content = "Приоритетность: " + priorety[i];
                prioretyLabel.FontSize = smallFont;
                Grid.SetRow(prioretyLabel, 3);
                Grid.SetColumn(prioretyLabel, 1);

                el.Children.Add(sell);
                el.Children.Add(phoneLabel);
                el.Children.Add(prioretyLabel);
                el.Children.Add(leftSide);
                el.Children.Add(agentNameLabel);

                Border Border1 = new Border();
                Border1.BorderThickness = new Thickness(1);
                Border1.BorderBrush = bgcolor;
                Border1.Margin = new Thickness(0, 0, 25, 15);

                RowDefinition rowDef = new RowDefinition();
                rowDef.MinHeight = 125;
                rowDef.MaxHeight = 125;
                rowDef.Name = $"row{i}";
                list.RowDefinitions.Add(rowDef);
                Grid.SetRow(el, i);
                Grid.SetColumnSpan(el, 3);
                list.Children.Add(el);
                Grid.SetRow(Border1, i);
                Grid.SetColumnSpan(Border1, 3);
                list.Children.Add(Border1);
                el.UpdateLayout();
            }
            list.UpdateLayout();
        }
    }
}
