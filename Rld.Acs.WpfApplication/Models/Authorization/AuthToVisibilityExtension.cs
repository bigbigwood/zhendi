using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Collections.Generic;
using Rld.Acs.WpfApplication.Service.Authorization;

namespace Rld.Acs.WpfApplication.Models.Authorization
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class AuthToVisibilityExtension : MarkupExtension
    {
        public string Operation { get; set; }

        public AuthToVisibilityExtension()
        {
            Operation = String.Empty;
        }

        public AuthToVisibilityExtension(string operation)
        {
            Operation = operation;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Operation))
                return Visibility.Collapsed;

            if (AuthProvider.Instance.CheckAccess(Operation))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
