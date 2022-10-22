using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PriyatniyShelestWPF
{
    /// <summary>
    /// Логика взаимодействия для AgentsEditWindow.xaml
    /// </summary>
    public partial class AgentsEditWindow : Window
    {
        public Agent currentAgent { get; set; } = new Agent();
        public bool isAddingNew { get; set; }

        static string basePath = AppDomain.CurrentDomain.BaseDirectory;
        static string[] config = File.ReadAllLines($"{basePath}\\connection.config");
        static string connStr = $"{config[0]}; {config[1]}; {config[2]}";

        public AgentsEditWindow()
        {
            InitializeComponent();
        }

        private void insertQuery()
        {
            /*
             * string queryString = "INSERT INTO Agent (Title, AgentTypeID, Priority, Logo, Address, INN, KPP, Phone, Email) VALUES" +
             * $" ()";
            */
        }

        private void alterQuery()
        {
            string queryString = "";
        }

        private void updateTypes(AgentType[] types)
        {
            agentTypeNew.Items.Clear();
            for (int i = 1; i < types.Length; i++)
            {
                agentTypeNew.Items.Add(types[i].Title);
            }
        }

        public void updateValues(AgentType[] types)
        {
            updateTypes(types);
            string actionText;
            if (isAddingNew) { actionText = "Добавление"; }
            else { actionText = $"Изменение агента {currentAgent.Title}"; }
            CurrentAction.Text = actionText;
            agentTitleNew.Text = currentAgent.Title;
            agentTypeNew.SelectedItem = currentAgent.AgentType;
            agentPriorityNew.Text = Convert.ToString(currentAgent.Priority);
            agentLogoPathNew.Text = currentAgent.Logo;
        }



        private void saveAgentProperties(object sender, RoutedEventArgs e)
        {
            if (isAddingNew) { insertQuery(); }
            else { alterQuery(); }
        }

        private void selectImagePath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            file.Multiselect = false;
            if (file.ShowDialog() == true)
            {
                File.Copy(file.FileName, basePath + $@"agents\{file.FileName}");
                agentLogoPathNew.Text = file.FileName;
            }
        }
    }
}