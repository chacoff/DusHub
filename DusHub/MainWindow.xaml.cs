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

        private bool isDarkMode = false;
        private readonly RegistryHandler _registryHandler;
        public ICommand CloseCommand { get; private set; }

        public MainWindow()
        {
            
            InitializeComponent();
            
            FillGrid();
            
            _registryHandler = new RegistryHandler();

            CloseCommand = new RelayCommand(_ => this.Close());
            this.DataContext = this;
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

        private void Launch_RightClick(object sender, RoutedEventArgs e)
        {
            string folder = GetFullFolder();
            var folderSplit = folder.Split('\\', 2);  // to avoid de drive letter
            string newFolder = folderSplit[1];

            if (newFolder.Substring(0, 1) == "\\")
            {
                newFolder = folderSplit[1].Remove(0,1);
            }
            
            string textINI = File.ReadAllText("resources/luxscan_base.ini");
            textINI = textINI.Replace("__base__", newFolder);
            File.WriteAllText("resources/luxscan.ini", textINI);

            string loadingBat = ConfigurationManager.AppSettings["BatApp"].ToString();
            string loadingDir = loadingBat.TrimEnd('\\').Remove(loadingBat.LastIndexOf('\\'));
            
            Process proc = new Process();
            proc.StartInfo.FileName = loadingBat;
            proc.StartInfo.WorkingDirectory = loadingDir;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Arguments = folder;
            proc.Start();
        }

        private void Rename_RightClick(object sender, RoutedEventArgs e)
        {
            string folder = GetFullFolder();
            Debug.WriteLine(folder);
        }

        private void Pattern_RightClick(object sender, RoutedEventArgs e)
        {
            //
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

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            if (isDarkMode)
            {
                SwitchToLightMode();
            }
            else
            {
                SwitchToDarkMode();
            }
            isDarkMode = !isDarkMode;
        }

        private void SwitchToDarkMode()
        {
            ResourceDictionary darkTheme = new ResourceDictionary
            {
                Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(darkTheme);

            ResourceDictionary windowStyle = new ResourceDictionary
            {
                Source = new Uri("Themes/WindowStyle.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(windowStyle);

            ThemeToggleIcon.Source = new BitmapImage(new Uri("resources/light.png", UriKind.Relative));
        }

        private void SwitchToLightMode()
        {
            ResourceDictionary lightTheme = new ResourceDictionary
            {
                Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(lightTheme);

            ResourceDictionary windowStyle = new ResourceDictionary
            {
                Source = new Uri("Themes/WindowStyle.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(windowStyle);

            ThemeToggleIcon.Source = new BitmapImage(new Uri("resources/dark.png", UriKind.Relative));
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
