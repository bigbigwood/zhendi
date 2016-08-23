﻿using System;
using GalaSoft.MvvmLight;
using log4net;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceOperationLogViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual Int32 LogID { get; set; }
        public virtual Int32? DeviceUserId { get; set; }
        public virtual Int32 DeviceId { get; set; }
        public virtual Int32 DeviceType { get; set; }
        public virtual Int32 OperationType { get; set; }
        public virtual String OperationDescription { get; set; }
        public virtual Int32 OperatorId { get; set; }
        public virtual String OperationContent { get; set; }
        public virtual DateTime? OperationTime { get; set; }
        public virtual DateTime? OperationUploadTime { get; set; }


    }
}