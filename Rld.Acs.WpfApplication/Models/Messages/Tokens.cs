﻿using System;

namespace Rld.Acs.WpfApplication.Models.Messages
{
    public class Tokens
    {
        public static readonly string OpenUserView = Guid.NewGuid().ToString();
        public static readonly string CloseUserView = Guid.NewGuid().ToString();
        public static readonly string UserPage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string UserPage_ShowQuestion = Guid.NewGuid().ToString();
        public static readonly string UserView_ShowNotification = Guid.NewGuid().ToString();

        public static readonly string OpenDepartmentView = Guid.NewGuid().ToString();
        public static readonly string CloseDepartmentView = Guid.NewGuid().ToString();
        public static readonly string DepartmentPage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string DepartmentPage_ShowQuestion = Guid.NewGuid().ToString();
        public static readonly string DepartmentView_ShowNotification = Guid.NewGuid().ToString();

        public static readonly string OpenTimeSegmentView = Guid.NewGuid().ToString();
        public static readonly string CloseTimeSegmentView = Guid.NewGuid().ToString();
        public static readonly string TimeSegmentPage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string TimeSegmentPage_ShowQuestion = Guid.NewGuid().ToString();
        public static readonly string TimeSegmentView_ShowNotification = Guid.NewGuid().ToString();

        public static readonly string OpenTimeGroupView = Guid.NewGuid().ToString();
        public static readonly string CloseTimeGroupView = Guid.NewGuid().ToString();
        public static readonly string TimeGroupPage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string TimeGroupPage_ShowQuestion = Guid.NewGuid().ToString();
        public static readonly string TimeGroupView_ShowNotification = Guid.NewGuid().ToString();

        public static readonly string OpenTimeZoneDashboardView = Guid.NewGuid().ToString();
        public static readonly string OpenTimeZoneView = Guid.NewGuid().ToString();
        public static readonly string CloseTimeZoneView = Guid.NewGuid().ToString();
        public static readonly string TimeZonePage_ShowNotification = Guid.NewGuid().ToString();
        public static readonly string TimeZonePage_ShowQuestion = Guid.NewGuid().ToString();
        public static readonly string TimeZoneView_ShowNotification = Guid.NewGuid().ToString();

    }
}