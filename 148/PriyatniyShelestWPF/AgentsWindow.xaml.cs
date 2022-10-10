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

namespace PriyatniyShelestWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // sortType True >> 12345...
        // sortType False >> 54321...
        bool sortType = true;

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "") 
            {
                SearchBox.Text = "Поиск";
            }
        }

        private void ChangeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (ChangeOrderButton.Content)
            {
                case "↓↓": ChangeOrderButton.Content = "↑↑"; sortType = true; break;
                case "↑↑": ChangeOrderButton.Content = "↓↓"; sortType = false; break;
            }
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
