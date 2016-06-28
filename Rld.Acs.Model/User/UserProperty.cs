using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserProperty
    {
        public Int32 UserPropertyID { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String Nationality { get; set; }
        public String NativePlace { get; set; }
        public DateTime Birthday { get; set; }
        public Int32? Marriage { get; set; }
        public Int32? PoliticalStatus { get; set; }
        public Int32? Degree { get; set; }
        public String HomeNumber { get; set; }
        public String EnglishName { get; set; }
        public String Company { get; set; }
        public String TechnicalTitle { get; set; }
        public String TechnicalLevel { get; set; }
        public Int32 IDType { get; set; }
        public String IDNumber { get; set; }
        public String SocialNumber { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String Postcode { get; set; }
        public String Remark { get; set; }
    }
}
