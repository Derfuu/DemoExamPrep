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

        // sortOrder True >> 12345...
        // sortOrder False >> 54321...
        Agent[] agents = new Agent[0];
        int possibleRows = 10;

        string connStr = "Data Source=DESKTOP-0000001; Initial Catalog=priyatniyDEV; Integrated Security=TRUE";

        /*
         FUNCTIONS
        */

        void updateTableConfiguretion()
        {
            double rowThikness = 150;
            
            centerGrid.RowDefinitions.Clear();

            for (int row = 0; row < possibleRows; row++)
            {
                RowDefinition rowDef = new RowDefinition();
                Border agentDataBorder = new Border();
                Label agentNameLabel = new Label();
                Label agentDiscountLabel = new Label();

                rowDef.MinHeight = rowThikness;
                rowDef.MaxHeight = rowThikness;
                rowDef.Name = $"row{row}";

                //border settings
                agentDataBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC6, 0xD7, 0xFF));
                agentDataBorder.Name = $"border{row}";
                Grid.SetRow(agentDataBorder, row);

                //agentNameLabel settings
                agentNameLabel.Content = $"AgentName{row}";
                agentNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                agentNameLabel.VerticalAlignment = VerticalAlignment.Top;
                agentNameLabel.Margin = new Thickness(10);
                agentNameLabel.Width = Double.NaN;
                Grid.SetRow(agentNameLabel, row);

                //agentDiscountLabel settings
                int discount = row * 10;
                agentDiscountLabel.Content = $"{discount}%";
                agentDiscountLabel.HorizontalAlignment = HorizontalAlignment.Right;
                agentDiscountLabel.VerticalAlignment = VerticalAlignment.Center;
                agentDiscountLabel.Margin = new Thickness(20);
                agentDiscountLabel.Width = Double.NaN;
                Grid.SetRow(agentDiscountLabel, row);

                //create instanses for grid
                centerGrid.RowDefinitions.Add(rowDef);
                centerGrid.Children.Add(agentDataBorder);
                centerGrid.Children.Add(agentNameLabel);
                centerGrid.Children.Add(agentDiscountLabel);
            }
            centerGrid.UpdateLayout();
        }

        Agent[] getAgentsFromDB(string connectionString)
        {
            return agents;
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
    }
}
