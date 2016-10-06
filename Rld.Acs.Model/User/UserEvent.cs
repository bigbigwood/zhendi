using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserEvent
    {
        public virtual Int64 EventId { get; set; }
        public virtual UserEventType EventType { get; set; }
        public virtual String EventData { get; set; }
        public virtual Int32 UserID { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Boolean IsFinished { get; set; }
    }
}
