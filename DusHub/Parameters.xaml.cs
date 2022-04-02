using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DusHub
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class Parameters : Window
    {
        MainWindow parent;

        public Parameters(MainWindow parent)
        {  
            InitializeComponent();
            Load_Parameters();
            this.parent = parent;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // @"DusHub.dll"

            config.AppSettings.Settings["LufApp"].Value = txtLufBox.Text;
            config.AppSettings.Settings["BatApp"].Value = txtTarget.Text;
            config.AppSettings.Settings["RegeditApp"].Value = txtRegedit.Text;
            config.AppSettings.Settings["KeyValueApp"].Value = txtRegeditKey.Text;
            config.AppSettings.Settings["RipValueApp"].Value = txtRipValue.Text;
            config.AppSettings.Settings["ChopValueApp"].Value = txtChopValue.Text;
            config.AppSettings.Settings["KeyYieldApp"].Value = txtYieldKey.Text;
            config.AppSettings.Settings["RipYieldApp"].Value = txtRipYield.Text;
            config.AppSettings.Settings["ChopYieldApp"].Value = txtChopYield.Text;

            config.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("appSettings");
            parent.FillGrid();
            this.Close();

            // Process.Start(Application.ResourceAssembly.Location);
            // Application.Current.Shutdown();
        }

        void Load_Parameters()
        {
            var appSet = ConfigurationManager.AppSettings;

            txtLufBox.Text = appSet["LufApp"].ToString();
            txtTarget.Text = appSet["BatApp"].ToString();
            txtRegedit.Text = appSet["RegeditApp"].ToString();
            txtRegeditKey.Text = appSet["KeyValueApp"].ToString();
            txtRipValue.Text = appSet["RipValueApp"].ToString();
            txtChopValue.Text = appSet["ChopValueApp"].ToString();
            txtYieldKey.Text = appSet["KeyYieldApp"].ToString();
            txtRipYield.Text = appSet["RipYieldApp"].ToString();
            txtChopYield.Text = appSet["ChopYieldApp"].ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
