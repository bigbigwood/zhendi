using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rld.Acs.WpfApplication.CustomerControl
{
    public class ImageButton : Button
    {
        private string imagepath;

        public string ImgPath
        {
            get { return imagepath; }
            set { imagepath = value; }
        }
    }
}
