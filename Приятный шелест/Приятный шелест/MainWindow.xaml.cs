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
        int Page = 1;
        int Paginator = 10;
        int YearsRange = 5;
        int MaxPage = 0;
        private void GetMaxPage()
        {
            SqlCommand command = new SqlCommand("select max(Agent.ID) from agent", db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                MaxPage = reader.GetInt32(0);
            }
            if (MaxPage % 10 != 0)
            {
                MaxPage = MaxPage / 10 + 1;
            }
            else
            {
                MaxPage = MaxPage / 10;
            }
            reader.Close();
        }
        //rgb(151, 255, 122);
        SolidColorBrush green = new SolidColorBrush(Color.FromArgb(50, 151, 255, 122));
        SolidColorBrush red = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
        SolidColorBrush invisible = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private void Window_Initialized(object sender, EventArgs e)
        {
            GetMaxPage();
            string queryString = $"SELECT Agent.Title, AgentType.Title,Agent.Phone, Agent.[Priority],Agent.Logo, (SELECT ISNULL(SUM(ProductSale.ProductCount), 0) FROM ProductSale WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'Sales',(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)), 0) " +
               $"FROM ProductSale, Product WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'TotalSalesBy'  FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) " +
               $"where Agent.ID between {Paginator - 9} and {Paginator} ";
            Zapros(queryString);
        }
        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            if (Page + 1 > MaxPage)
            {
                buttonRight.Background = red;
            }
            else
            {
                buttonLeft.Background = invisible;
                buttonRight.Background = invisible;
                Page += 1;
                Paginator += 10;
                DestroyContent();
                string queryString = $"SELECT Agent.Title, AgentType.Title,Agent.Phone, Agent.[Priority],Agent.Logo, (SELECT ISNULL(SUM(ProductSale.ProductCount), 0) FROM ProductSale WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'Sales',(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)), 0) " +
                $"FROM ProductSale, Product WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'TotalSalesBy'  FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) " +
                $"where Agent.ID between {Paginator - 9} and {Paginator}";
                Zapros(queryString, true);
            }
        }
        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (Page - 1 <= 0)
            {
                buttonLeft.Background = red;
            }
            else
            {
                buttonLeft.Background = invisible;
                buttonRight.Background = invisible;
                Page -= 1;
                Paginator -= 10;
                string queryString = $"SELECT Agent.Title, AgentType.Title,Agent.Phone, Agent.[Priority],Agent.Logo, (SELECT ISNULL(SUM(ProductSale.ProductCount), 0) FROM ProductSale WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'Sales',(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)), 0) " +
                $"FROM ProductSale, Product WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'TotalSalesBy'  FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) " +
                $"where Agent.ID between {Paginator - 9} and {Paginator}";
                Zapros(queryString, true);
            }
        }
        private void DestroyContent()
        {
            if (list != null)
            {
                list.RowDefinitions.Clear();
                list.Children.Clear();
            }
        }
        private void Zapros(string queryString1, bool page = false)
        {
            if (page)
            {
                PageInfo.Content = $"Вы на {Page} из {MaxPage}";
            }
            DestroyContent();
            string[] name = new string[10];
            int[] prod = new int[10];
            string[] phone = new string[10];
            int[] priorety = new int[10];
            decimal[] priceProd = new decimal[10];
            string[] logo = new string[10];
            SqlCommand command = new SqlCommand(queryString1, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                name[i] = reader.GetString(1) + " | " + reader.GetString(0);
                phone[i] = reader.GetString(2);
                priorety[i] = reader.GetInt32(3);
                logo[i] = reader.GetString(4);
                prod[i] = reader.GetInt32(5);
                priceProd[i] = reader.GetDecimal(6);
                i++;
            }
            reader.Close();
            int smallFont = 15;
            int bigFont = 16;
            SolidColorBrush bgcolor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            for (i = 0; i < name.Length; i++)
            {
                if (name[i] == null)
                {
                    return;
                }
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
                if (logo[i] == "none" || logo[i] == "отсутствует")
                {
                    leftSide.Source = new BitmapImage(new Uri("/picture.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                }
                else
                {
                    //MessageBox.Show(logo[i]);
                    leftSide.Source = new BitmapImage(new Uri(logo[i], UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                }
                Grid.SetRowSpan(leftSide, 4);
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

                Border Border1 = new Border();
                Border1.BorderThickness = new Thickness(1);
                Border1.BorderBrush = bgcolor;
                Border1.Margin = new Thickness(0, 0, 25, 15);

                //prod
                Label sell = new Label();
                sell.Content = prod[i] + " Продаж за год";
                if (priceProd[i] * prod[i] <= 10000)
                {
                    discount.Content = "0%";
                }
                if (priceProd[i] * prod[i] > 10000 && priceProd[i] * prod[i] <= 50000)
                {
                    discount.Content = "5%";
                }
                else if (priceProd[i] * prod[i] > 50000 && priceProd[i] * prod[i] <= 150000)
                {
                    discount.Content = "10%";
                }
                else if (priceProd[i] * prod[i] > 150000 && priceProd[i] * prod[i] <= 500000)
                {
                    discount.Content = "20%";
                }
                else if (priceProd[i] * prod[i] > 500000)
                {
                    discount.Content = "25%";
                    Border1.Background = green;
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
            }
            list.UpdateLayout();
            
        }
        bool firstInit = false;
        private void filter(object sender, SelectionChangedEventArgs e)
        {
            string queryString = $"SELECT Agent.Title, AgentType.Title,Agent.Phone, Agent.[Priority],Agent.Logo, (SELECT ISNULL(SUM(ProductSale.ProductCount), 0) FROM ProductSale WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'Sales',(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)), 0) " +
                $"FROM ProductSale, Product WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < {YearsRange}) AS 'TotalSalesBy'  FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) ";
            if (FilterBox.SelectedIndex == 0)
            {
                if (!firstInit)
                {
                    firstInit = true;
                    return;
                }
                Zapros(queryString + $"where AgentType.Title between {Paginator - 9} and {Paginator} ");
            }
            else if (FilterBox.SelectedIndex == 1)
            {
                queryString += $"where AgentType.Title like 'ООО'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
            else if (FilterBox.SelectedIndex == 2)
            {
                queryString += $"where AgentType.Title like 'ПАО'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
            else if (FilterBox.SelectedIndex == 3)
            {
                queryString += $"where AgentType.Title like 'ОАО'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
            else if (FilterBox.SelectedIndex == 4)
            {
                queryString += $"where AgentType.Title like 'МФО'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
            else if (FilterBox.SelectedIndex == 5)
            {
                queryString += $"where AgentType.Title like 'ЗАО'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
            else if (FilterBox.SelectedIndex == 6)
            {
                queryString += $"where AgentType.Title like 'МКК'" +
                    $" order by Agent.Title OFFSET {Paginator - 9} ROWS FETCH NEXT {Paginator} ROWS ONLY";
                Zapros(queryString);
            }
        }
    }
}
