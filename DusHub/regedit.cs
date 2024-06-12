using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DusHub
{
    public class RegistryHandler
    {
        public void Rip_Checked(CheckBox rip)
        {
            var appSet = ConfigurationManager.AppSettings;

            rip.Content = "Ripscan (uncheck for Chop)";
            RegistryKey _localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64); // RegistryView.Registry32
            var localreg = _localKey.OpenSubKey(appSet["RegeditApp"], true);

            localreg ??= _localKey.CreateSubKey(appSet["RegeditApp"]);
            localreg.SetValue(appSet["KeyValueApp"], appSet["RipValueApp"]);
            localreg.SetValue(appSet["KeyYieldApp"], appSet["RipYieldApp"]);
            localreg.Close();
        }

        public void Rip_Unchecked(CheckBox rip)
        {
            var appSet = ConfigurationManager.AppSettings;

            rip.Content = "Ripscan";
            RegistryKey _localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64); // RegistryView.Registry32
            var localreg = _localKey.OpenSubKey(appSet["RegeditApp"], true);

            localreg ??= _localKey.CreateSubKey(appSet["RegeditApp"]);
            localreg.SetValue(appSet["KeyValueApp"], appSet["ChopValueApp"]);
            localreg.SetValue(appSet["KeyYieldApp"], appSet["ChopYieldApp"]);
            localreg.Close();
        }
    }
}
