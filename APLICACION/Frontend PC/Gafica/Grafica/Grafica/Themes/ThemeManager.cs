using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grafica.Themes
    {
        public static class ThemeManager
        {
            public static void ApplyTheme(string themeName)
            {
                string path = $"Themes/{themeName}Theme.xaml";

                var dict = new ResourceDictionary
                {
                    Source = new Uri(path, UriKind.Relative)
                };

                // Remueve diccionarios anteriores
                var appResources = Application.Current.Resources.MergedDictionaries;
                appResources.Clear();
                appResources.Add(dict);
            }
        }
    }
