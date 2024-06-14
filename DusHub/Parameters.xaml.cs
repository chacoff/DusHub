using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.IO;
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

        private void btnOpenFolderDialog_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                txtLufBox.Text = openFileDlg.SelectedPath;
            }
        }

        private void btnOpenBatDialog_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Batch files (*.bat)|*.bat|Text files (*.txt)|*.txt|All files (*.*)|*.*";
                // openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtTarget.Text = openFileDialog.FileName;
                    //filePath = openFileDialog.FileName;
                    
                    //var fileStream = openFileDialog.OpenFile();

                    //using (StreamReader reader = new StreamReader(fileStream))
                    //{
                    //    fileContent = reader.ReadToEnd();
                    //}
                }
            }

            //System.Windows.Forms.MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // @"DusHub.dll"

            string _theme;

            if (themeBox.IsChecked == true){
                _theme = "true";
            }
            else
            {
                _theme = "false";
            }

            config.AppSettings.Settings["ThemeApp"].Value = _theme;
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
            Theme.SwitchTheme(Convert.ToBoolean(_theme));
            parent.ThemeToggleIcon.Source = Theme.GetThemeIcon(Convert.ToBoolean(_theme));
            this.Close();

            // Process.Start(Application.ResourceAssembly.Location);
            // Application.Current.Shutdown();
        }

        void Load_Parameters()
        {
            var appSet = ConfigurationManager.AppSettings;

            themeBox.IsChecked = Convert.ToBoolean(appSet["ThemeApp"]);
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
