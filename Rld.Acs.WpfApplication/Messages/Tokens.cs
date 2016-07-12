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

        public static readonly string Summary_GotoSutff = Guid.NewGuid().ToString();
        public static readonly string Summary_GotoDepartment = Guid.NewGuid().ToString();
        public static readonly string Summary_GotoDevice = Guid.NewGuid().ToString();
        public static readonly string Summary_GotoDoor = Guid.NewGuid().ToString();

        public static readonly string OpenDepartmentView = Guid.NewGuid().ToString();
        public static readonly string CloseDepartmentView = Guid.NewGuid().ToString();
        public static readonly string DepartmentPage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string DepartmentPage_ShowQuestion = Guid.NewGuid().ToString();
    }
}
