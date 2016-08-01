using Rld.DeviceSystem.Contract.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation
{
    public class UserEnrollSummary
    {
        public UserRole Role { get; set; }
        public Boolean PasswordEnabled{ get; set; }
        public Boolean CardEnabled { get; set; }
        public Boolean FingerPrint0Enabled { get; set; }
        public Boolean FingerPrint1Enabled { get; set; }
        public Boolean FingerPrint2Enabled { get; set; }
        public Boolean FingerPrint3Enabled { get; set; }
        public Boolean FingerPrint4Enabled { get; set; }
        public Boolean FingerPrint5Enabled { get; set; }
        public Boolean FingerPrint6Enabled { get; set; }
        public Boolean FingerPrint7Enabled { get; set; }
        public Boolean FingerPrint8Enabled { get; set; }
        public Boolean FingerPrint9Enabled { get; set; }

    }
}