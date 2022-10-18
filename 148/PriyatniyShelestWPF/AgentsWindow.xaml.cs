using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace PriyatniyShelestWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
         GLOBAL PROPERTIES 
        */

        Agent[] agents;
        AgentType[] agentTypes;

        SolidColorBrush bgcolor = new SolidColorBrush(Color.FromArgb(0xFF, 0xC6, 0xD7, 0xFF));
        SolidColorBrush bgcolor25 = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xFF, 0x00));

        string connStr = "Data Source=DESKTOP-0000001; Initial Catalog=priyatniyDEVmain; Integrated Security=TRUE";
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        int currentPage = 1;

        /*
         FUNCTIONS
        */

        void updateTableConfiguretion()
        {
            agents = new Agent[getAgentsFromDB(connStr, onPage: currentPage).Length];
            getAgentsFromDB(connStr, onPage: currentPage).CopyTo(agents, 0);

            centerGrid.RowDefinitions.Clear();
            centerGrid.Children.Clear();

            double rowThikness = 150;
            int itemsInList = 10;

            for (int i = 0; i < itemsInList; i++)
            {
                SolidColorBrush usingBgColor;
                if (agents[i].getDiscount() == 25)
                { usingBgColor = bgcolor25; }
                else { usingBgColor = bgcolor; }
                {
                    Grid innerGrid = new Grid();
                    Border agentDataBorder = new Border();
                    Border agentLogoBorder = new Border();
                    Image agentLogo = new Image();
                    Label agentDiscountLabel = new Label();
                    Label agentNameLabel = new Label();

                    {
                        innerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                        ColumnDefinition imageColumn = new ColumnDefinition();
                        ColumnDefinition descriptionColumn = new ColumnDefinition();
                        ColumnDefinition discountColumn = new ColumnDefinition();
                        imageColumn.Width = new GridLength(200);
                        descriptionColumn.Width = new GridLength(1, GridUnitType.Star);
                        discountColumn.Width = new GridLength(100);

                        innerGrid.ColumnDefinitions.Add(imageColumn);
                        innerGrid.ColumnDefinitions.Add(descriptionColumn);
                        innerGrid.ColumnDefinitions.Add(discountColumn);

                        Grid.SetRow(innerGrid, i);
                    }// record creation

                    {
                        agentDataBorder.Background = bgcolor;
                        agentLogoBorder.Background = bgcolor;
                        Grid.SetColumn(agentLogoBorder, 0);
                        Grid.SetColumn(agentDataBorder, 1);
                        innerGrid.Children.Add(agentLogoBorder);
                        innerGrid.Children.Add(agentDataBorder);
                    }// border setting

                    {
                        Uri fileUri = new Uri(basePath + agents[i].Logo);
                        agentLogo.Source = new BitmapImage(fileUri);
                        Grid.SetColumn(agentLogo, 0);
                        innerGrid.Children.Add(agentLogo);
                    }// image setting

                    {
                        
                        agentNameLabel.Content = $"{agents[i].AgentType}|{agents[i].Title}";
                        agentNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                        agentNameLabel.VerticalAlignment = VerticalAlignment.Top;
                        agentNameLabel.Margin = new Thickness(10);
                        agentNameLabel.Width = Double.NaN;
                        Grid.SetColumn(agentNameLabel, 1);
                        innerGrid.Children.Add(agentNameLabel);
                    }//agentNameLabel setting

                    {
                        int discount = agents[i].getDiscount();
                        agentDiscountLabel.Content = $"{discount}%";
                        agentDiscountLabel.HorizontalAlignment = HorizontalAlignment.Right;
                        agentDiscountLabel.VerticalAlignment = VerticalAlignment.Center;
                        agentDiscountLabel.Margin = new Thickness(20);
                        agentDiscountLabel.Width = Double.NaN;
                        Grid.SetColumn(agentDiscountLabel, 1);
                        innerGrid.Children.Add(agentDiscountLabel);
                    }//agentDiscountLabel setting

                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = new GridLength(150);
                        rowDef.Name = $"row{i}";
                        centerGrid.RowDefinitions.Add(rowDef);
                        innerGrid.UpdateLayout();
                        centerGrid.Children.Add(innerGrid);
                    }// inserting ready row in grid
                }
            }
            centerGrid.UpdateLayout();
        } //wrong width

        void updateFilterTypes(string connectionString)
        {
            agentTypes = getAgentTypes(connectionString);
            FilterBox.Items.Clear();
            FilterBox.Items.Add("Без фильтра");
            for(int i = 1; i < agentTypes.Length; i++)
            {
                FilterBox.Items.Insert(agentTypes[i].ID, agentTypes[i].Title);
            }
        }

        AgentType[] getAgentTypes(string connectionString)
        {
            string queryString = "SELECT COUNT(*) FROM AgentType";
            int LenOfTypes = 1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    LenOfTypes = reader.GetInt32(0);
                }
                reader.Close();
            }
            AgentType[] Type = new AgentType[LenOfTypes + 1];
            Type[0] = new AgentType();
            Type[0].ID = 0;
            Type[0].Title = "None";

            queryString = "SELECT AgentType.ID, AgentType.Title FROM AgentType";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int id = 1;
                while (reader.Read())
                {
                    Type[id] = new AgentType{
                        ID = reader.GetInt32(0),
                        Title = reader.GetString(1)
                    };
                    id++;
                };
                reader.Close();
            }
            return Type;
        }

        Agent[] getAgentsFromDB(string connectionString, string searchFor="", string inFilter="%", int sortBy=0, int onPage = 1, int recordsPerPage = 10)
        {
            if (searchFor == "") { searchFor = "%"; } //Search all

            string searchForQuery = $"AND (Agent.Title LIKE '{searchFor}' " +
                $"OR Agent.Email LIKE '{searchFor}' " +
                $"OR Agent.Phone LIKE '{searchFor}') ";

            string recordsToGetQuery = $"OFFSET {(onPage * recordsPerPage) - recordsPerPage} ROWS FETCH NEXT {recordsPerPage} ROWS ONLY ";

            string sortType = sortType = "Agent.Title ";
            switch (sortBy)
            {
                case 0: { sortType = "Agent.Title "; ; break; }
                case 1: { sortType = "Sales ASC "; break; } // Sales
                case 2: { sortType = "TotalSalesBy ASC "; break; } // Discount
                case 3: { sortType = "Agent.Priority "; break; }
            }
            
            int salesForYear = 10;
            string queryString = "SELECT Agent.ID, AgentType.Title AS 'Type', Agent.Title, " +
                "Agent.[Address], Agent.INN, Agent.KPP, " +
                "Agent.DirectorName, Agent.Phone, Agent.[Priority], " +
                "Agent.Email, Agent.Logo, " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount), 0) " +
                "FROM ProductSale " +
                $"WHERE ProductSale.AgentID = Agent.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYear}) AS 'Sales', " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount * Product.MinCostForAgent), 0) " +
                "FROM ProductSale, Product " +
                $"WHERE ProductSale.AgentID = Agent.ID AND ProductSale.ProductID = Product.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYear}) AS 'TotalSalesBy'" +
                $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.Title LIKE('{inFilter}') " +
                searchForQuery +
                $"ORDER BY {sortType} " + recordsToGetQuery;

            Agent[] recivedAgents = new Agent[recordsPerPage];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                int id = 0;
                while (reader.Read())
                {
                    recivedAgents[id] = new Agent
                    {
                        ID = reader.GetInt32(0),
                        AgentType = reader.GetString(1),
                        Title = reader.GetString(2),
                        Address = reader.GetString(3),
                        INN = reader.GetString(4),
                        KPP = reader.GetString(5),
                        DirectorName = reader.GetString(6),
                        Phone = reader.GetString(7),
                        Priority = reader.GetInt32(8),
                        Email = reader.GetString(9),
                        Logo = reader.GetString(10),
                        Sales = reader.GetInt32(11),
                        TotalSalesBy = reader.GetDecimal(12)
                    };
                    id++;
                }
                reader.Close();
            }
            return recivedAgents;
        }

        /*
         EVENT HANDLERS 
        */



        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateTableConfiguretion();
        }

        private void ChangeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (ChangeOrderButton.Content)
            {
                case "↓↓": ChangeOrderButton.Content = "↑↑"; break;
                case "↑↑": ChangeOrderButton.Content = "↓↓"; break;
            }
            agents.Reverse();
            updateTableConfiguretion();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortBox.SelectedIndex != 0)
            {
                updateTableConfiguretion();
            }
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterBox.SelectedIndex != 0)
            {
                updateTableConfiguretion();
            }
        }

        private void updateAgentsButton_Click(object sender, RoutedEventArgs e)
        {
            updateTableConfiguretion();
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateTableConfiguretion();
        }

        private void window_Initialized(object sender, EventArgs e)
        {
            updateFilterTypes(connStr);
            updateTableConfiguretion();
        }
    }
}

