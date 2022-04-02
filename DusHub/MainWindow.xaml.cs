using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Win32;
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

namespace DusHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            FillGrid();
        }

        public class Data2Grid
        {
            public string? Name { get; set; }

            public string? LastWriteTime { get; set; }
        }

        public void FillGrid()
        {
            var appSet = ConfigurationManager.AppSettings;
            string Luf = appSet["LufApp"].ToString();
            string[] splitLuf = Luf.Split('/');
            List<Data2Grid> paths = new List<Data2Grid>();
            DirectoryInfo[]? fileDir;

            if (Directory.Exists(Luf))
            {
                fileDir = new DirectoryInfo(Luf).GetDirectories();
                    if (splitLuf.Length > 3)
                    {
                        txtLuf.Content = splitLuf[0] + "/ ... /" + splitLuf[^2] + "/" + splitLuf[^1];
                    }
                    else
                    {
                        txtLuf.Content = Luf;
                    }
            }
            else
            {
                fileDir = new DirectoryInfo(@"c:\").GetDirectories();
                txtLuf.Content = @"C:\";
            }
            
            foreach (var fi in fileDir)
            {
                paths.Add(new Data2Grid { Name = fi.Name, LastWriteTime = fi.LastWriteTime.ToString()});
            }

            MainGrid.ItemsSource = paths;
        }

        private void Grid_RightClick(object sender, RoutedEventArgs e)
        {
            //
            var cellInfo = MainGrid.SelectedCells[0];
            var cell = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
            Debug.WriteLine(cell.Text);
        }

        private void params_Click(object sender, RoutedEventArgs e)
        {
            Parameters win2 = new(this);
            win2.ShowDialog();
        }

        private void Rip_Checked(object sender, RoutedEventArgs e)
        {
            var appSet = ConfigurationManager.AppSettings;

            Rip.Content = "Ripscan (uncheck for Chop)";

            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64); // RegistryView.Registry32
            localKey = localKey.OpenSubKey(appSet["RegeditApp"], true);
            
            if (localKey != null)
            {
                localKey.SetValue(appSet["KeyValueApp"], appSet["RipValueApp"]);
                localKey.SetValue(appSet["KeyYieldApp"], appSet["RipYieldApp"]);
            }

            localKey.Close();
        }

        private void Rip_Unchecked(object sender, RoutedEventArgs e)
        {
            var appSet = ConfigurationManager.AppSettings;

            Rip.Content = "Ripscan";

            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64); // RegistryView.Registry32
            localKey = localKey.OpenSubKey(appSet["RegeditApp"], true);

            if (localKey != null)
            {
                localKey.SetValue(appSet["KeyValueApp"], appSet["ChopValueApp"]);
                localKey.SetValue(appSet["KeyYieldApp"], appSet["ChopYieldApp"]);
            }

            localKey.Close();
        }
    }
}
