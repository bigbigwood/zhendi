﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Messages
{
    public class Tokens
    {
        public static readonly string OpenCustomerView = Guid.NewGuid().ToString();
        public static readonly string CloseCustomerView = Guid.NewGuid().ToString();
    }
}
