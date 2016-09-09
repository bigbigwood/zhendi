using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.LisenceTool
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LisenceUnit
    {
        [Description("年")]
        Year = 0,
        [Description("月")]
        Month = 1,
        [Description("天")]
        Day = 2,
    }
}
