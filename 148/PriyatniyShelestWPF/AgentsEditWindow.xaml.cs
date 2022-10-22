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
        Agent currentAgent;
        AgentType[] agentTypes;
        bool isAddingNew = false;
        static string basePath = AppDomain.CurrentDomain.BaseDirectory;
        static string[] config = System.IO.File.ReadAllLines($"{basePath}\\connection.config");
        string connStr = $"{config[0]}; {config[1]}; {config[2]}";
        private AgentsEditWindow(AgentType[] usedAgentTypes, Agent currentAgentUsing = null, bool isAddNew=false)
        {
            InitializeComponent();
            currentAgent = currentAgentUsing;
            usedAgentTypes.CopyTo(agentTypes, 0);
            isAddingNew = isAddNew;
        }

        static void insertQuery()
        {
            /*
             * string queryString = "INSERT INTO Agent (Title, AgentTypeID, Priority, Logo, Address, INN, KPP, Phone, Email) VALUES" +
             * $" ()";
            */
        }

        static void alterQuery()
        {
            string queryString = "";
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
            if (file.ShowDialog() == DialogResult)
            {
                File.Copy(file.FileName, basePath + @"\agents\");
                agentLogoPathNew.Text = file.FileName;
            }
        }
    }
}