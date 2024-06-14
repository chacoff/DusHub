using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;

namespace DusHub
{
    public class Theme
    {

        private static string? theme;

        public static void SwitchTheme(bool isDarkTheme)
        {
            // var theme = isDarkTheme ? "Themes/LightTheme.xaml" : "Themes/DarkTheme.xaml";

            if (isDarkTheme){
                theme = "Themes/DarkTheme.xaml";
            }
            else
            {
                theme = "Themes/LightTheme.xaml";
            }

            ResourceDictionary newTheme = new ResourceDictionary() { Source = new Uri(theme, UriKind.Relative) };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }

        public static BitmapImage GetThemeIcon(bool isDarkTheme)
        {
            string themeIcon;

            // isDarkTheme ? "Resources/dark.png" : "Resources/light.png";

            if (!isDarkTheme)
            {
                themeIcon = "resources/dark.png";
            }
            else
            {
                themeIcon = "resources/light.png";
            }

            return new BitmapImage(new Uri(themeIcon, UriKind.Relative));
        }
    }
}
