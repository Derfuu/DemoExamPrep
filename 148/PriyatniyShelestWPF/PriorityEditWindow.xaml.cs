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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace PriyatniyShelestWPF
{
    /// <summary>
    /// Логика взаимодействия для PriorityEditWindow.xaml
    /// </summary>
    public partial class PriorityEditWindow : Window
    {
        public PriorityEditWindow()
        {
            InitializeComponent();
        }

        private static bool IsTextAllowed(string text)
        {
            Regex _regex = new Regex("[^0-9]"); //regex that matches disallowed text
            return !_regex.IsMatch(text);
        }
        private void saveProperties(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выполнено", "Внимание");
            this.Close();
        }

        private void priorityTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = priority.Text;
            if (!IsTextAllowed(text)) { SaveAgentData.IsEnabled = false; }
            else { SaveAgentData.IsEnabled = true; }
        }
    }
}
