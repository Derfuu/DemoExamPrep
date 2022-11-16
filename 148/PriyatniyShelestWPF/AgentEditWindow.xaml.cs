using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace PriyatniyShelestWPF
{
    /// <summary>
    /// Логика взаимодействия для AgentsEditWindow.xaml
    /// </summary>
    public partial class AgentsEditWindow : Window
    {
        public Agent currentAgent { get; set; } = new Agent();
        public bool isAddingNew { get; set; }
        public string connectionString { get; set; }

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
            string queryString = $@"UPDATE Agent SET Title='{agentTitleNew.Text}', AgentTypeID='{agentTypeNew.SelectedIndex + 1}', Priority='{agentPriorityNew.Text}',
                Logo='{agentLogoPathNew.Text}', Address='{agentAddressNew.Text}', INN='{agentInnNew.Text}', KPP='{agentKppNew.Text}', Phone='{agentPhoneNew.Text}',
                Email='{agentEmailNew.Text}' 
                WHERE Agent.ID = {currentAgent.ID}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Выполнено");
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
            agentAddressNew.Text = currentAgent.Address;
            agentInnNew.Text = currentAgent.INN;
            agentKppNew.Text = currentAgent.KPP;
            agentPhoneNew.Text = currentAgent.Phone;
            agentEmailNew.Text = currentAgent.Email;
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
                string source = file.FileName;
                File.Copy(file.FileName, basePath + $@"agents\{file.SafeFileName}");
                agentLogoPathNew.Text = $@"agents\{file.SafeFileName}";
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex _regex = new Regex("[^0-9]"); //regex that matches disallowed text
            return !_regex.IsMatch(text);
        }

        private void agentInnNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = agentInnNew.Text;
            if (!IsTextAllowed(text) || text.Length != 9) { SaveAgentData.IsEnabled = false; }
            else { SaveAgentData.IsEnabled = true; }
        }

        private void agentKppNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = agentKppNew.Text;
            if (!IsTextAllowed(text) || text.Length != 10) { SaveAgentData.IsEnabled = false; }
            else { SaveAgentData.IsEnabled = true; }
        }

        private void agentPhoneNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = agentPhoneNew.Text;
            if (!IsTextAllowed(text) || text.Length < 11) { SaveAgentData.IsEnabled = false; }
            else { SaveAgentData.IsEnabled = true; }
        }
    }
}