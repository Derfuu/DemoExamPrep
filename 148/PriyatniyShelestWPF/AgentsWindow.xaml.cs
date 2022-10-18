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

        string execPath = Assembly.GetEntryAssembly().Location;

        string connStr = "Data Source=DESKTOP-0000001; Initial Catalog=priyatniyDEVmain; Integrated Security=TRUE";

        /*
         FUNCTIONS
        */

        void updateTableConfiguretion(int page = 1)
        {
            double rowThikness = 150;
            
            centerGrid.RowDefinitions.Clear();

            int[] agentsSlice = new int[2];
            agentsSlice[0] = (page - 1) * 10;
            agentsSlice[1] = page * 10;

            int displayedRows = agentsSlice[1];

            for (int row = agentsSlice[0]; row < displayedRows; row++)
            {
                Grid innerGrid = new Grid();
                innerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                ColumnDefinition imageColumn = new ColumnDefinition();
                ColumnDefinition descriptionColumn = new ColumnDefinition();
                innerGrid.ColumnDefinitions.Add(imageColumn);
                innerGrid.ColumnDefinitions.Add(descriptionColumn);

                imageColumn.Width = new GridLength(200);
                descriptionColumn.Width = new GridLength();

                //image settings
                Image agentLogo = new Image();
                BitmapImage logo = new BitmapImage();

                logo.BeginInit();
                logo.UriSource = new Uri("agents/no_image.png", UriKind.Relative);
                agentLogo.Stretch = Stretch.UniformToFill;
                agentLogo.Source = logo;
                logo.EndInit();
                Grid.SetColumn(agentLogo, 0);


                //border settings
                Border agentDataBorder = new Border();
                Border agentLogoBorder = new Border();
                agentDataBorder.Background = bgcolor;
                agentLogoBorder.Background = bgcolor;
                Grid.SetColumn(agentLogoBorder, 0);
                Grid.SetColumn(agentDataBorder, 1);


                //agentNameLabel settings
                Label agentNameLabel = new Label();

                agentNameLabel.Content = $"AgentName{row}";
                agentNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                agentNameLabel.VerticalAlignment = VerticalAlignment.Top;
                agentNameLabel.Margin = new Thickness(10);
                agentNameLabel.Width = Double.NaN;
                Grid.SetColumn(agentNameLabel, 1);


                //agentDiscountLabel settings
                Label agentDiscountLabel = new Label();

                int discount = row * 10;
                agentDiscountLabel.Content = $"{discount}%";
                agentDiscountLabel.HorizontalAlignment = HorizontalAlignment.Right;
                agentDiscountLabel.VerticalAlignment = VerticalAlignment.Center;
                agentDiscountLabel.Margin = new Thickness(20);
                agentDiscountLabel.Width = Double.NaN;
                Grid.SetColumn(agentDiscountLabel, 1);


                //create instanses for grid
                innerGrid.Children.Add(agentLogo);
                innerGrid.Children.Add(agentLogoBorder);
                innerGrid.Children.Add(agentDataBorder);
                innerGrid.Children.Add(agentNameLabel);
                innerGrid.Children.Add(agentDiscountLabel);
                Grid.SetRow(innerGrid, row);

                //inserting ready row in grid
                RowDefinition rowDef = new RowDefinition();

                rowDef.MinHeight = rowThikness;
                rowDef.MaxHeight = rowThikness;
                rowDef.Name = $"row{row}";
                centerGrid.RowDefinitions.Add(rowDef);
                innerGrid.UpdateLayout();
                centerGrid.Children.Add(innerGrid);
            }
            centerGrid.UpdateLayout();
        } //wrong width

        void updateFilterTypes(string connectionString)
        {
            getAgentTypes(connectionString);
            FilterBox.Items.Clear();
            FilterBox.Items.Add("Без фильтра");
            for(int i = 0; i < agentTypes.Length; i++)
            {
                FilterBox.Items.Add("Заглушка");
                FilterBox.Items.Insert(agentTypes[i].ID, agentTypes[i].Title);
            }
        }

        void getAgentTypes(string connectionString)
        {
            string queryString = "SELECT COUNT(*) FROM AgentType";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    agentTypes = new AgentType[reader.GetInt32(0)];
                }
                reader.Close();
            }

            queryString = "SELECT AgentType.ID, AgentType.Title FROM AgentType";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int id = 1;
                while (reader.Read())
                {
                    agentTypes[id] = new AgentType();
                    agentTypes[id].ID = new int();
                    agentTypes[id].ID = reader.GetInt32(0);
                    agentTypes[id].Title = reader.GetString(1);
                    //MessageBox.Show("" + agentTypes[id].ID + agentTypes[id].Title);
                }
                reader.Close();
            }
        }

        void getAgentsFromDB(string connectionString, string searchFor="", int filterBy=0, int sortBy=0)
        {
            string sortType = "Agent.Title";
            string filterByType = "%";
            switch (sortBy)
            {
                case 0: { sortType = "Agent.Title "; ; break; }
                case 1: { sortType = "Sales ASC"; break; } // Sales
                case 2: { sortType = "TotalSalesBy ASC"; break; } // Discount
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
                                $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.Title LIKE('{filterByType}') " +
                                $"ORDER BY {sortType}";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                }
                reader.Close();
            }
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
            getAgentsFromDB(connStr);
        }
    }
}
