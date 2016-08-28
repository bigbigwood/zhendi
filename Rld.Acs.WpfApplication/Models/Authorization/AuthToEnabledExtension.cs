using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Collections.Generic;
using Rld.Acs.WpfApplication.Service.Authorization;

namespace Rld.Acs.WpfApplication.Models.Authorization
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class AuthToEnabledExtension : MarkupExtension
    {
        public string Operation { get; set; }

        public AuthToEnabledExtension()
        {
            Operation = String.Empty;
        }

        public AuthToEnabledExtension(string operation)
        {
            Operation = operation;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Operation))
                return false;

            return AuthProvider.Instance.CheckAccess(Operation);
        }
    }
}
