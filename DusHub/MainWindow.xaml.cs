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
using static DusHub.MainWindow;

namespace DusHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDarkTheme = Convert.ToBoolean(ConfigurationManager.AppSettings["ThemeApp"]);
        private readonly RegistryHandler _registryHandler;

        public MainWindow()
        {
            
            InitializeComponent();
            
            FillGrid();

            _registryHandler = new RegistryHandler();

            Theme.SwitchTheme(isDarkTheme);

            ThemeToggleIcon.Source = Theme.GetThemeIcon(isDarkTheme);
        }

        public void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void FillGrid()
        {
            var appSet = ConfigurationManager.AppSettings;
            string Luf = appSet["LufApp"].ToString();
            string[] splitLuf = Luf.Split('\\');

            List<Data2Grid> paths = new List<Data2Grid>();
            DirectoryInfo[]? fileDir;

            if (Directory.Exists(Luf))
            {
                fileDir = new DirectoryInfo(Luf).GetDirectories();
                // fileDir = Directory.GetDirectories(Luf).Contains(System.IO.Path.Combine(Luf, "Test_Folder"));

                if (splitLuf.Length > 2)
                {
                    txtLuf.Content = "...\\" + splitLuf[^1];
                }
                else
                {
                    txtLuf.Content = Luf;
                }
                txtLog.Content = "OK";
                txtLog.Foreground = new SolidColorBrush(Color.FromRgb(0, 132, 0));
            }
            else
            {
                fileDir = new DirectoryInfo(@"c:\").GetDirectories();
                txtLuf.Content = @"C:\";
                txtLog.Content = "Error";
                txtLog.Foreground = new SolidColorBrush(Color.FromRgb(160, 0, 5));
            }

            foreach (var fi in fileDir)
            {
                paths.Add(new Data2Grid { Name = fi.Name, LastWriteTime = fi.LastWriteTime.ToString() });
            }

            MainGrid.ItemsSource = paths;
        }

        private string getNewFolder(string folder)
        {
            var folderSplit = folder.Split('\\', 2);  // to avoid de drive letter
            string newFolder = folderSplit[1];

            if (newFolder.Substring(0, 1) == "\\")
            {
                newFolder = folderSplit[1].Remove(0, 1);
            }

            return newFolder;
        }

        private void getINIready(string newFolder)
        {
            string textINI = File.ReadAllText("resources/luxscan_base.ini");
            textINI = textINI.Replace("__base__", newFolder);
            File.WriteAllText("resources/luxscan.ini", textINI);
        }

        private void Launcher_endpoint(string bat, string dir, string folder)
        {
            Process proc = new Process();

            proc.StartInfo.FileName = bat;
            proc.StartInfo.WorkingDirectory = dir;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Arguments = folder;

            #if DEBUG
                Console.WriteLine(folder);
            #endif

            proc.Start();
        }

        private void launcherPipeline()
        {
            string folder = GetFullFolder();

            string newFolder = getNewFolder(folder);

            getINIready(newFolder);

            string loadingBat = ConfigurationManager.AppSettings["BatApp"].ToString();
            string loadingDir = loadingBat.TrimEnd('\\').Remove(loadingBat.LastIndexOf('\\'));
            
            Launcher_endpoint(loadingBat, loadingDir, folder);
        }

        private void Launch_RightClick(object sender, RoutedEventArgs e)
        {
            launcherPipeline();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                launcherPipeline();
            }
        }

        private void params_Click(object sender, RoutedEventArgs e)
        {
            Parameters win2 = new Parameters(this);
            win2.ShowDialog();
        }

        public string GetFullFolder()
        {
            string Luf = ConfigurationManager.AppSettings["LufApp"].ToString();
            var cellInfo = MainGrid.SelectedCells[0];
            var cell = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
            string result = Luf + "\\" + cell.Text;

            return result;
        }

        private void Rip_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox Rip)
            {
                _registryHandler.Rip_Checked(Rip);
            }
        }

        private void Rip_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox Rip)
            {
                _registryHandler.Rip_Unchecked(Rip);
            }
        }

        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isDarkTheme = !isDarkTheme;

            Theme.SwitchTheme(isDarkTheme);

            ThemeToggleIcon.Source = Theme.GetThemeIcon(isDarkTheme);

        }
    }
}
