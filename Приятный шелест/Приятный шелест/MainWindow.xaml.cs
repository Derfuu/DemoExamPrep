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
        int YearsRange = 10;
        private void Window_Initialized(object sender, EventArgs e)
        {
            string queryString1 = "select [AgentType].Title, [Agent].[Title], [Phone], [Priority]" +
            $"from[Agent] INNER JOIN[AgentType] ON [Agent].[AgentTypeID] = [AgentType].[ID]  where Agent.ID between {Paginator - 10} and {Paginator}";
            string queryString2 = "select [ProductSale].AgentID, [ProductSale].ProductCount, [Product].MinCostForAgent " +
            "from [Product], [Agent] " +
            $"INNER JOIN[ProductSale] ON [Agent].ID = [ProductSale].AgentID where Agent.ID between {Paginator - 10} and {Paginator}" +
            $"and  DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange} and [ProductSale].ProductID = [Product].ID" +
            $" ORDER BY AgentID";
            Zapros(queryString1, queryString2);
        }
        private void DestroyContent()
        {
            //gridDel10
            for (int i = 0; i < controlsGrid.Length; i++)
            {
                //controls[i].= 0;
                //controlsGrid[i].Children.Remove(controlsRows[i]);
                list.Children.Remove(controlsGrid[i]);
                //list.Children.Remove(controlsRows[i]);
            }
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            DestroyContent();
            string queryString1 = "select [AgentType].Title, [Agent].[Title], [Phone], [Priority]" +
            $"from[Agent] INNER JOIN[AgentType] ON [Agent].[AgentTypeID] = [AgentType].[ID]  where Agent.ID between {Paginator - 10} and {Paginator}";
            string queryString2 = "select [ProductSale].AgentID, [ProductSale].ProductCount, [Product].MinCostForAgent " +
            "from [Product], [Agent] " +
            $"INNER JOIN[ProductSale] ON [Agent].ID = [ProductSale].AgentID where Agent.ID between {Paginator - 10} and {Paginator}" +
            $"and  DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange} and [ProductSale].ProductID = [Product].ID" +
            $" ORDER BY AgentID";
            Zapros(queryString1, queryString2);
        }
        Grid[] controlsGrid = new Grid[10];
        RowDefinition[] controlsRows = new RowDefinition[10];
        private void Zapros(string queryString1, string queryString2)
        {
            string[] name = new string[10];
            int[] prod = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] prodID = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            string[] phone = new string[10];
            int[] priorety = new int[10];
            decimal[] priceProd = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            SqlCommand command = new SqlCommand(queryString1, db.getConnection());
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
            command = new SqlCommand(queryString2, db.getConnection());
            db.openConnection();
            reader = command.ExecuteReader();
            i = 0;
            while (reader.Read())
            {
                prodID[i] = reader.GetInt32(0);
                prod[i] = reader.GetInt32(1);
                priceProd[i] = reader.GetDecimal(2);
                i++;
            }
            reader.Close();
            int idProdBack = -2;
            int ContProdFirst = -1;
            int smallFont = 12;
            int bigFont = 15;
            SolidColorBrush bgcolor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            for (i = 0; i < 10; i++)
            {
                Grid el = new Grid();
                ColumnDefinition img = new ColumnDefinition();
                ColumnDefinition descript = new ColumnDefinition();
                ColumnDefinition procent = new ColumnDefinition();
                img.Width = new GridLength(110);
                descript.Width = new GridLength(1, GridUnitType.Star);
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
                el.ColumnDefinitions.Add(procent);

                //agentNameLabel settings
                Image leftSide = new Image();
                leftSide.Source = new BitmapImage(new Uri("/picture.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                Grid.SetRowSpan(leftSide, 4);
                leftSide.Height = 100;
                leftSide.Width = 100;
                leftSide.Margin = new Thickness(5);
                leftSide.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetColumn(leftSide, 0);

                //agentNameLabel settings
                Label agentNameLabel = new Label();
                agentNameLabel.Content = name[i];
                agentNameLabel.FontSize = bigFont;
                Grid.SetRow(agentNameLabel, 0);
                Grid.SetColumn(agentNameLabel, 1);

                //discount settings
                Label discount = new Label();
                discount.Content = "0 %";
                discount.FontSize = bigFont;
                //Grid.SetRow(discount, 0);
                discount.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(discount, 1);
                Grid.SetRowSpan(discount, 2);
                Grid.SetColumn(discount, 2);

                for (int j = 0; j < 10; j++)
                {
                    if (prodID[j] == Paginator - 9 + i)
                    {
                        if (idProdBack == prodID[j])
                        {
                            prod[ContProdFirst] = prod[ContProdFirst] + prod[j];
                            priceProd[ContProdFirst] = priceProd[ContProdFirst] + priceProd[j];
                            prodID[j] = -1;
                            prod[j] = -1;
                            priceProd[j] = 0;
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
                        if (priceProd[j] * prod[j] <= 10000)
                        {
                            discount.Content = "0%";
                        }
                        if (priceProd[j] * prod[j] > 10000 && priceProd[j] * prod[j] <= 50000)
                        {
                            discount.Content = "5%";
                        }
                        else if (priceProd[j] * prod[j] > 50000 && priceProd[j] * prod[j] <= 150000)
                        {
                            discount.Content = "10%";
                        }
                        else if (priceProd[j] * prod[j] > 150000 && priceProd[j] * prod[j] <= 500000)
                        {
                            discount.Content = "20%";
                        }
                        else if (priceProd[j] * prod[j] > 500000)
                        {
                            discount.Content = "25%";
                        }
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

                el.Children.Add(discount);
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
                rowDef.Name = $"rowDel{i}";
                el.Name = $"gridDel{i}";
                list.RowDefinitions.Add(rowDef);
                Grid.SetRow(el, i);
                Grid.SetColumnSpan(el, 3);
                list.Children.Add(el);
                Grid.SetRow(Border1, i);
                Grid.SetColumnSpan(Border1, 3);
                list.Children.Add(Border1);
                el.UpdateLayout();
                //controls.Add("gridDel{i}", el);
                controlsGrid[i] = el;
                controlsRows[i] = rowDef;
            }
            list.UpdateLayout();
            
        }
    }
}
