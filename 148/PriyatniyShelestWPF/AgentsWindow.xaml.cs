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

        string connStr = "Data Source=LAPTOP-TK7JKUOV; Initial Catalog=test; Integrated Security=TRUE";
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        int currentPage = 1;
        bool reverse = false;
        /*
         FUNCTIONS
        */

        void updateTableConfiguration(bool isReverse=false)
        {
            int agentsLen = getAgentsFromDB(connStr,
                onPage: currentPage,
                searchFor: SearchBox.Text,
                inFilter: FilterBox.SelectedIndex).Length;

            agents = new Agent[agentsLen];

            agents = getAgentsFromDB(connStr,
                onPage: currentPage,
                searchFor: SearchBox.Text,
                inFilter: FilterBox.SelectedIndex,
                sortBy: SortBox.SelectedIndex,
                reversed: isReverse);

            if (agents == null || agents[0] == null) { return; }

            centerGrid.RowDefinitions.Clear();
            centerGrid.Children.Clear();
            
            for (int i = 0; i < agentsLen; i++)
            {
                SolidColorBrush usingBgColor;
                if (agents[i].getDiscount() == 25)
                { usingBgColor = bgcolor25; }
                else { usingBgColor = bgcolor; }
                {
                    Grid innerGrid = new Grid();
                    Grid descriptionsGrid = new Grid();

                    Border agentDataBorder = new Border();
                    Border agentLogoBorder = new Border();

                    Image agentLogo = new Image();

                    Label agentDiscountLabel = new Label();

                    Label agentTypeNameLabel = new Label();
                    Label agentSalesLabel = new Label();
                    Label agentPhoneLabel = new Label();
                    Label agentPriorityLabel = new Label();

                    {
                        innerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                        descriptionsGrid.VerticalAlignment = VerticalAlignment.Stretch;

                        ColumnDefinition imageColumn = new ColumnDefinition();
                        imageColumn.Width = new GridLength(200);
                        innerGrid.ColumnDefinitions.Add(imageColumn);

                        ColumnDefinition descriptionColumn = new ColumnDefinition();
                        descriptionColumn.Width = new GridLength(1, GridUnitType.Star);
                        innerGrid.ColumnDefinitions.Add(descriptionColumn);

                        ColumnDefinition discountColumn = new ColumnDefinition();
                        discountColumn.Width = new GridLength(100);
                        innerGrid.ColumnDefinitions.Add(discountColumn);

                        RowDefinition TypeRow = new RowDefinition();
                        TypeRow.Height = new GridLength(30);
                        descriptionsGrid.RowDefinitions.Add(TypeRow);

                        RowDefinition TitleRow = new RowDefinition();
                        TitleRow.Height = new GridLength(30);
                        descriptionsGrid.RowDefinitions.Add(TitleRow);

                        RowDefinition PhoneRow = new RowDefinition();
                        PhoneRow.Height = new GridLength(30);
                        descriptionsGrid.RowDefinitions.Add(PhoneRow);

                        RowDefinition PriorityRow = new RowDefinition();
                        PriorityRow.Height = new GridLength(30);
                        descriptionsGrid.RowDefinitions.Add(PriorityRow);

                        Grid.SetColumn(descriptionsGrid, 1);
                        Grid.SetRow(innerGrid, i);
                    }// record creation

                    {
                        agentDataBorder.Background = usingBgColor;
                        agentLogoBorder.Background = usingBgColor;
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
                        agentTypeNameLabel.Content = $"{agents[i].AgentType} | {agents[i].Title}  ";
                        Grid.SetRow(agentTypeNameLabel, 0);
                        agentSalesLabel.Content = $"{agents[i].Sales}  ";
                        Grid.SetRow(agentSalesLabel, 1);
                        agentPhoneLabel.Content = $"{agents[i].Phone}  ";
                        Grid.SetRow(agentPhoneLabel, 2);
                        agentPriorityLabel.Content = $"Приоритетность: {agents[i].Priority}  ";
                        Grid.SetRow(agentPriorityLabel, 3);

                        descriptionsGrid.Children.Add(agentTypeNameLabel);
                        descriptionsGrid.Children.Add(agentSalesLabel);
                        descriptionsGrid.Children.Add(agentPhoneLabel);
                        descriptionsGrid.Children.Add(agentPriorityLabel);
                    }// agentDescription setting

                    {
                        int discount = agents[i].getDiscount();
                        agentDiscountLabel.Content = $"{discount}%";

                        Grid.SetColumn(agentDiscountLabel, 2);
                        innerGrid.Children.Add(agentDiscountLabel);
                    }// agentDiscountLabel setting

                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = new GridLength(150);
                        rowDef.Name = $"row{i}";
                        centerGrid.RowDefinitions.Add(rowDef);
                        innerGrid.Children.Add(descriptionsGrid);
                        innerGrid.UpdateLayout();
                        descriptionsGrid.UpdateLayout();
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
            FilterBox.SelectedIndex = 0;
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

        Agent[] getAgentsFromDB(string connectionString, string searchFor="", int inFilter=0, int sortBy=0, int onPage = 1, int recordsPerPage = 10, bool reversed=false)
        {
            if (searchFor == "") { searchFor = "%"; } //Search all

            string searchForQuery = $"AND ((Agent.Title LIKE '{searchFor}' " +
                $"OR Agent.Email LIKE '{searchFor}' " +
                $"OR Agent.Phone LIKE '{searchFor}')) ";

            string recordsToGetQuery = $"OFFSET {(onPage * recordsPerPage) - recordsPerPage} ROWS FETCH NEXT {recordsPerPage} ROWS ONLY ";

            string sortType = "Agent.Title ";

            string filter;
            if (inFilter == 0) { filter = "%"; }
            else { filter = (string)FilterBox.Items[inFilter]; }
            
            switch (sortBy)
            {
                case 0: { sortType = "Agent.Title "; ; break; }
                case 1: { sortType = "Sales "; break; } // Sales
                case 2: { sortType = "TotalSalesBy "; break; } // Discount
                case 3: { sortType = "Agent.Priority "; break; }
            }
            if (sortBy != 2) 
                if( !reversed) { sortType += "DESC "; }
                else { sortType += "ASC "; }
            else
                if (reversed) { sortType += "DESC "; }
            else { sortType += "ASC "; }

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
                $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.Title LIKE('{filter}') " +
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

        void revereTextCheck()
        {
            if (reverse) { ChangeOrderButton.Content = "↓↓"; }
            else { ChangeOrderButton.Content = "↑↑"; }
        }

        /*
         EVENT HANDLERS 
        */

        private void ChangeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            reverse = !reverse;
            revereTextCheck();
            if (SortBox != null && FilterBox != null)
            { updateTableConfiguration(reverse); }
        }

        private void agentsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            updateFilterTypes(connStr);
            if (SortBox != null && FilterBox != null)
            { updateTableConfiguration(); revereTextCheck(); }
        }

        private void SortType_Changed(object sender, RoutedEventArgs e)
        {
            if (SortBox != null && FilterBox != null)
            { updateTableConfiguration(); revereTextCheck(); }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SortBox != null && FilterBox != null && SearchBox.Text != null)
            { updateTableConfiguration(); revereTextCheck(); }
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortBox != null && FilterBox != null)
            { updateTableConfiguration(); revereTextCheck(); }
        }
    }
}

