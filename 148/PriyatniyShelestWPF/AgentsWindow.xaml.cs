using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        static string basePath = AppDomain.CurrentDomain.BaseDirectory;
        static string[] config = System.IO.File.ReadAllLines($"{basePath}\\connection.config");
        string connStr = $"{config[0]}; {config[1]}; {config[2]}";
        int currentPage = 1;
        int maxPage = 1;
        List<CheckBox> CB_records = new List<CheckBox>();
        List<int> recordsToAlternate = new List<int>();
        bool reverseSorting = false;
        bool firstLoad = true;

        /*
         FUNCTIONS
        */
        bool is_deletion_available(int agent_id)
        {
            return false;
        }

        public void updateTableConfiguration()
        {
            string searchString = SearchBox.Text;

            int filterIndex = FilterBox.SelectedIndex;
            if (filterIndex == -1) { filterIndex = 0; }

            int sortIndex = SortBox.SelectedIndex;
            if (sortIndex == -1) { sortIndex = 0; }

            int totalPages = getMaxPages(connStr, searchFor: searchString, inFilter: filterIndex);
            maxPage = totalPages;
            int agentsLen = getAgentsFromDB(connStr, onPage: currentPage, searchFor: searchString, inFilter: filterIndex).Length;

            agents = new Agent[agentsLen];
            agents = getAgentsFromDB(connStr, onPage: currentPage, searchFor: searchString, inFilter: filterIndex, sortBy: sortIndex, reversed: reverseSorting);

            currentPageText.Content = ""+currentPage;
            centerGrid.RowDefinitions.Clear();
            centerGrid.Children.Clear();

            for (int i = 0; i < agentsLen; i++)
            {
                CB_records.Clear();
                SolidColorBrush usingBgColor;
                if (agents[i] == null) { break; }
                if (agents[i].getDiscount() == 25)
                { usingBgColor = bgcolor25; }
                else { usingBgColor = bgcolor; }

                {
                    Grid innerGrid = new Grid();
                    Grid descriptionsGrid = new Grid();
                    Grid discountGrid = new Grid();

                    Border agentDataBorder = new Border();
                    Border agentLogoBorder = new Border();

                    Image agentLogo = new Image();

                    Label agentDiscountLabel = new Label();
                    Label agentTypeNameLabel = new Label();
                    Label agentSalesLabel = new Label();
                    Label agentContactsLabel = new Label();
                    Label agentPriorityLabel = new Label();

                    Button editAgentButton = new Button();
                    Button deleteAgentButton = new Button();

                    CheckBox agentSelection = new CheckBox();

                    void addToAlterList(object sender, EventArgs e)
                    {
                        recordsToAlternate.Add(agents[(int)agentSelection.Tag].ID);
                    }//functions for buttons and checkers

                    void removeFromAlterList(object sender, EventArgs e)
                    {
                        recordsToAlternate.Remove(agents[(int)agentSelection.Tag].ID);
                    }//functions for buttons and checkers

                    void delAgent(object sender, EventArgs e)
                    {
                        ;
                        if (MessageBox.Show($"Удалить {agents[(int)deleteAgentButton.Tag].Title} ?", "Удаление агента",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            using (SqlConnection connection = new SqlConnection(connStr))
                            {
                                string queryString = $@"DELETE CASCADE FROM Agent WHERE Agent.ID='{agents[(int)deleteAgentButton.Tag].ID}'";
                                SqlCommand command = new SqlCommand(queryString, connection);
                                connection.Open();
                                if (is_deletion_available(agents[(int)deleteAgentButton.Tag].ID)) { command.ExecuteNonQuery(); }
                            }
                            MessageBox.Show("Выполнено");
                        }
                    }//functions for buttons and checkers

                    void alterAgent(object sender, EventArgs e)
                    {
                        AgentsEditWindow editWindow = new AgentsEditWindow();
                        editWindow.connectionString = connStr;
                        editWindow.currentAgent = agents[(int)deleteAgentButton.Tag];
                        editWindow.isAddingNew = false;
                        editWindow.updateValues(agentTypes);
                        editWindow.ShowDialog();
                    }//functions for buttons and checkers

                    {
                        innerGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                        descriptionsGrid.VerticalAlignment = VerticalAlignment.Stretch;

                        descriptionsGrid.Margin = new Thickness(5);
                        discountGrid.Margin = new Thickness(5);

                        ColumnDefinition imageColumn = new ColumnDefinition();
                        imageColumn.Width = new GridLength(200);
                        innerGrid.ColumnDefinitions.Add(imageColumn);

                        ColumnDefinition descriptionColumn = new ColumnDefinition();
                        descriptionColumn.Width = new GridLength(1, GridUnitType.Star);
                        innerGrid.ColumnDefinitions.Add(descriptionColumn);

                        ColumnDefinition propertiesColumn = new ColumnDefinition();
                        propertiesColumn.Width = new GridLength(200);
                        innerGrid.ColumnDefinitions.Add(propertiesColumn);

                        {
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
                        }//description column

                        {
                            ColumnDefinition propsColumnOne = new ColumnDefinition();
                            propsColumnOne.Width = new GridLength(1, GridUnitType.Star);
                            discountGrid.ColumnDefinitions.Add(propsColumnOne);

                            ColumnDefinition propsColumnTwo = new ColumnDefinition();
                            propsColumnTwo.Width = new GridLength(1, GridUnitType.Star);
                            discountGrid.ColumnDefinitions.Add(propsColumnTwo);

                            RowDefinition discountRow = new RowDefinition();
                            discountRow.Height = new GridLength(1, GridUnitType.Star);
                            discountGrid.RowDefinitions.Add(discountRow);

                            RowDefinition propsRowOne = new RowDefinition();
                            propsRowOne.Height = new GridLength(1, GridUnitType.Star);
                            discountGrid.RowDefinitions.Add(propsRowOne);
                        }//discount column

                        Grid.SetColumn(descriptionsGrid, 1);
                        Grid.SetColumn(discountGrid, 2);
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
                        agentLogo.Margin = new Thickness(10);
                        Grid.SetColumn(agentLogo, 0);
                        innerGrid.Children.Add(agentLogo);
                    }// image setting

                    {
                        agentTypeNameLabel.Content = $"{agents[i].AgentType} | {agents[i].Title}  ";
                        Grid.SetRow(agentTypeNameLabel, 0);
                        agentSalesLabel.Content = $"Продажи: {agents[i].Sales}  ";
                        Grid.SetRow(agentSalesLabel, 1);
                        agentContactsLabel.Content = $"Телефон: {agents[i].Phone} Email: {agents[i].Email}";
                        Grid.SetRow(agentContactsLabel, 2);
                        agentPriorityLabel.Content = $"Приоритетность: {agents[i].Priority}  ";
                        Grid.SetRow(agentPriorityLabel, 3);

                        descriptionsGrid.Children.Add(agentTypeNameLabel);
                        descriptionsGrid.Children.Add(agentSalesLabel);
                        descriptionsGrid.Children.Add(agentContactsLabel);
                        descriptionsGrid.Children.Add(agentPriorityLabel);
                    }// agentDescription setting

                    {
                        int discount = agents[i].getDiscount();

                        Thickness thicc = new Thickness(2);

                        Grid.SetColumn(agentDiscountLabel, 0);
                        Grid.SetRow(agentDiscountLabel, 0);
                        agentDiscountLabel.Content = $"{discount}%";
                        discountGrid.Children.Add(agentDiscountLabel);

                        Grid.SetColumn(agentSelection, 1);
                        Grid.SetRow(agentSelection, 0);
                        if (recordsToAlternate.Contains(agents[i].ID)) { agentSelection.IsChecked = true; }
                        CB_records.Add(agentSelection);
                        agentSelection.Tag = i;
                        agentSelection.HorizontalAlignment = HorizontalAlignment.Stretch;
                        agentSelection.VerticalAlignment = VerticalAlignment.Stretch;
                        agentSelection.HorizontalContentAlignment = HorizontalAlignment.Center;
                        agentSelection.Checked += addToAlterList;
                        agentSelection.Unchecked += removeFromAlterList;
                        agentSelection.Margin = thicc;
                        discountGrid.Children.Add(agentSelection);

                        Grid.SetColumn(editAgentButton, 0);
                        Grid.SetRow(editAgentButton, 1);
                        editAgentButton.Content = "Изменить";
                        editAgentButton.Click += alterAgent;
                        editAgentButton.Margin = thicc;
                        discountGrid.Children.Add(editAgentButton);

                        Grid.SetColumn(deleteAgentButton, 1);
                        Grid.SetRow(deleteAgentButton, 1);
                        deleteAgentButton.Content = "Удалить";
                        deleteAgentButton.Margin = thicc;
                        deleteAgentButton.Name = $"delBtn_{i}";
                        deleteAgentButton.Click += delAgent;
                        discountGrid.Children.Add(deleteAgentButton);
                    }// agentDiscountGrid setting

                    {
                        RowDefinition recordRow = new RowDefinition();
                        recordRow.Height = new GridLength(150);
                        recordRow.Name = $"row{i}";
                        centerGrid.RowDefinitions.Add(recordRow);
                        innerGrid.Children.Add(discountGrid);
                        innerGrid.Children.Add(descriptionsGrid);
                        discountGrid.UpdateLayout();
                        descriptionsGrid.UpdateLayout();
                        innerGrid.UpdateLayout();
                        centerGrid.Children.Add(innerGrid);
                    }// inserting ready row in grid
                }
            } // creating records on page
            centerGrid.UpdateLayout();
        }

        public void deselectAllAgents()
        {
            recordsToAlternate.Clear();
            updateTableConfiguration();
        }

        public int getMaxPages(string connectionString, string searchFor = "%", int inFilter = 0, int recordsPerPage = 10)
        {
            if (searchFor == "") { searchFor = "%"; } //Search all

            string searchForQuery = $"AND ((Agent.Title LIKE '%{searchFor}%' OR Agent.Email LIKE '%{searchFor}%' OR Agent.Phone LIKE '%{searchFor}%')) ";
            string filter;

            if (inFilter == 0) { filter = "%"; }
            else { filter = agentTypes[inFilter].ID.ToString(); }

            int salesForYears = 10;
            double maxPages;
            int records = 0;

            string queryString = "SELECT Agent.ID, AgentType.Title AS 'Type', Agent.Title, " +
                "Agent.[Address], Agent.INN, Agent.KPP, " +
                "Agent.DirectorName, Agent.Phone, Agent.[Priority], " +
                "Agent.Email, Agent.Logo, " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount), 0) " +
                "FROM ProductSale " +
                $"WHERE ProductSale.AgentID = Agent.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'Sales', " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount * Product.MinCostForAgent), 0) " +
                "FROM ProductSale, Product " +
                $"WHERE ProductSale.AgentID = Agent.ID AND ProductSale.ProductID = Product.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'TotalSalesBy'" +
                $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.ID LIKE '{filter}' " + searchForQuery;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    records++;
                }

                reader.Close();
            } // counts records
            maxPages = Math.Ceiling((float)records / 10);
            return (int)maxPages;
        }

        public void updateFilterTypes()
        {
            string connectionString = connStr;
            agentTypes = getAgentTypes(connectionString);
            FilterBox.Items.Clear();
            FilterBox.Items.Add("Все типы");
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
            Type[0].Title = "%";

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


        Agent[] getAgentsFromDB(string connectionString, string searchFor="%", int inFilter=0, int sortBy=0, int onPage = 1, int recordsPerPage = 10, bool reversed=false)
        {
            if (searchFor == "") { searchFor = "%"; } //Search all

            string searchForQuery = $"AND ((Agent.Title LIKE '%{searchFor}%' OR Agent.Email LIKE '%{searchFor}%' OR Agent.Phone LIKE '%{searchFor}%')) ";
            string sortType = "Agent.Title ";
            string paginatorSelectionQuery = $"OFFSET {(onPage * recordsPerPage) - recordsPerPage} ROWS FETCH NEXT {recordsPerPage} ROWS ONLY ";
            string filter;

            if (inFilter == 0) { filter = "%"; }
            else { filter = agentTypes[inFilter].ID.ToString(); }
            
            switch (sortBy)
            {
                case 0: { sortType = "Agent.Title "; ; break; }
                case 1: { sortType = "TotalSalesBy "; break; } // Discount
                case 2: { sortType = "Agent.Priority "; break; }
            }

            if (reversed) { sortType += "DESC "; }
            else { sortType += "ASC "; }
            
            int salesForYears = 10;

            string queryString = "SELECT Agent.ID, AgentType.Title AS 'Type', Agent.Title, " +
                "Agent.[Address], Agent.INN, Agent.KPP, " +
                "Agent.DirectorName, Agent.Phone, Agent.[Priority], " +
                "Agent.Email, Agent.Logo, " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount), 0) " +
                "FROM ProductSale " +
                $"WHERE ProductSale.AgentID = Agent.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'Sales', " +
                "(SELECT ISNULL(SUM(ProductSale.ProductCount * Product.MinCostForAgent), 0) " +
                "FROM ProductSale, Product " +
                $"WHERE ProductSale.AgentID = Agent.ID AND ProductSale.ProductID = Product.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'TotalSalesBy'" +
                $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.ID LIKE '{filter}' " + searchForQuery +
                "ORDER BY " + sortType + paginatorSelectionQuery;

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
            } // importing agents into array
            return recivedAgents;
        }

        public void reverseTextCheck()
        {
            if (reverseSorting) { ChangeOrderButton.Content = "↓↓"; }
            else { ChangeOrderButton.Content = "↑↑"; }
        }

        /*
         EVENT HANDLERS 
        */

        private void ChangeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            reverseSorting = !reverseSorting;
            reverseTextCheck();
            { currentPage = 1; updateTableConfiguration(); reverseTextCheck(); }
        }

        private void FilterSource_Changed(object sender, RoutedEventArgs e)
        {
            if (firstLoad == false)
            { currentPage = 1; updateTableConfiguration(); reverseTextCheck(); }
        }

        private void pagintorSelectionUP(object sender, RoutedEventArgs e)
        {
            if (currentPage != maxPage)
            { currentPage++; updateTableConfiguration(); }
        }

        private void pagintorSelectionDOWN(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
            { currentPage--; updateTableConfiguration(); }
        }

        private void window_init(object sender, EventArgs e)
        {
            firstLoad = !firstLoad;
            { updateFilterTypes(); updateTableConfiguration(); reverseTextCheck(); }
        }

        private void deselectAllAgentsBtn_Click(object sender, RoutedEventArgs e)
        {
            deselectAllAgents();
        }
    }
}

