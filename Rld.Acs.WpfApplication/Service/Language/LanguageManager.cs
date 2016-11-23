﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.Service.Language
{
    public static class LanguageManager
    {
        public static Languages DefaultLanguage = Languages.Chinese;

        public static void ChangeLanguage(Languages selectedLanguage)
        {
            //The name of the folder where the languages are contained (could be a parameter)
            string folder = "Localization";

            Uri source = null;
            if (selectedLanguage == Languages.English)
            {
                source = new Uri("/Rld.Acs.WpfApplication.Resources;component/Localization/English.xaml", UriKind.Relative);
            }
            else
            {
                source = new Uri("/Rld.Acs.WpfApplication.Resources;component/Localization/Chinese.xaml", UriKind.Relative);
            }

            ResourceDictionary dictionary;

            try
            {
                dictionary = (ResourceDictionary)Application.LoadComponent(source);
            }
            catch (IOException)
            {
                //resource file doesn't exist.
                return;
            }

            //Remove current resource from the merged dictionaries
            var currentResource = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source != null && x.Source.ToString().Contains(folder));
            if (currentResource != null )
            {
                if (currentResource.Contains(selectedLanguage.ToString()))
                    return;
                else
                {
                    Application.Current.Resources.MergedDictionaries.Remove(currentResource);
                }
            }

            //Add the new resource to the dictionary
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            
        }
    }
}