using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserProperty
    {
        public virtual Int32 UserPropertyID { get; set; }
        public virtual String LastName { get; set; }
        public virtual String FirstName { get; set; }
        public virtual Int32 Nationality { get; set; }
        public virtual String NativePlace { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual Marriage Marriage { get; set; }
        public virtual Int32? PoliticalStatus { get; set; }
        public virtual Int32? Degree { get; set; }
        public virtual String HomeNumber { get; set; }
        public virtual String EnglishName { get; set; }
        public virtual String Company { get; set; }
        public virtual String TechnicalTitle { get; set; }
        public virtual String TechnicalLevel { get; set; }
        public virtual Int32 IDType { get; set; }
        public virtual String IDNumber { get; set; }
        public virtual String SocialNumber { get; set; }
        public virtual String Email { get; set; }
        public virtual String Address { get; set; }
        public virtual String Postcode { get; set; }
        public virtual String Remark { get; set; }
    }
}
