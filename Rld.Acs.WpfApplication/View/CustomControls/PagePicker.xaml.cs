using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rld.Acs.WpfApplication.View.CustomControls
{
    /// <summary>
    /// PagePicker.xaml 的交互逻辑
    /// </summary>
    public partial class PagePicker : UserControl
    {
        //定义一个委托
        public delegate void PageChangedHandle(object sender, EventArgs e);
        //定义一个事件
        public event PageChangedHandle PageChanged;

        public PagePicker()
        {
            InitializeComponent();

        }
        //总页数
        private int totalPage = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        private int currentPage = 1;

        #region 每页显示的条数
        /// <summary>
        /// 注册当前页
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(String),
        typeof(PagePicker), new FrameworkPropertyMetadata("1", FrameworkPropertyMetadataOptions.AffectsMeasure), CurrentPageValidation);

        /// <summary>
        /// 验证当前页
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool PageSizeValidation(object value)
        {
            return true;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public string PageSize
        {
            get { return GetValue(PageSizeProperty).ToString(); }
            set
            {
                SetValue(PageSizeProperty, value);
                //lblPageSize.Text = value;
            }
        }
        #endregion

        #region 当前页
        /// <summary>
        /// 注册当前页
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(String),
        typeof(PagePicker), new FrameworkPropertyMetadata("1", FrameworkPropertyMetadataOptions.AffectsMeasure, OnCurrentPageChanged), CurrentPageValidation);

        /// <summary>
        /// 验证当前页
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CurrentPageValidation(object value)
        {
            return true;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public string CurrentPage
        {
            get { return GetValue(CurrentPageProperty).ToString(); }
            set
            {
                SetValue(CurrentPageProperty, value);

                lblCurrentPage.Text = value;
            }
        }


        #endregion

        #region 总页数
        /// <summary>
        /// 总页数
        /// </summary>
        public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(String),
            typeof(PagePicker), new FrameworkPropertyMetadata("1", FrameworkPropertyMetadataOptions.AffectsMeasure, OnTotalPageChanged), TotalPageValidation);

        /// <summary>
        /// 总页数进行验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TotalPageValidation(object value)
        {
            return true;
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public string TotalPage
        {
            get { return GetValue(TotalPageProperty).ToString(); }
            set
            {
                SetValue(TotalPageProperty, value);

            }
        }

        #endregion

        #region 转到第n页
        /// <summary>
        /// 注册转到第n页
        /// </summary>
        public static readonly DependencyProperty NavigationPageProperty = DependencyProperty.Register("NavigationPage", typeof(String),
        typeof(PagePicker), new FrameworkPropertyMetadata("1", FrameworkPropertyMetadataOptions.AffectsMeasure, OnNavigationPageChanged), NavigationPageValidation);

        /// <summary>
        /// 验证当前页
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NavigationPageValidation(object value)
        {
            return true;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public string NavigationPage
        {
            get { return GetValue(NavigationPageProperty).ToString(); }
            set
            {
                SetValue(NavigationPageProperty, value);
            }
        }


        #endregion
        #region 私有方法
        /// <summary>
        /// 值改变方法将由此方法来引发事件
        /// </summary>
        private void PageChangedFunc()
        {
            if (PageChanged != null)
            {
                ///引发事件
                PageChanged(this, new EventArgs());
            }
        }


        #endregion

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFristPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = "1";
            PageChangedFunc();

        }
        /// <summary>
        /// 前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            totalPage = GetIntVal(TotalPage);
            currentPage = GetIntVal(CurrentPage);
            if (currentPage > 1)
            {
                currentPage = currentPage - 1;
                CurrentPage = currentPage.ToString();
            }
            PageChangedFunc();
        }
        /// <summary>
        /// 后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = GetIntVal(CurrentPage);
            totalPage = GetIntVal(TotalPage);
            if (currentPage < totalPage)
            {
                currentPage = currentPage + 1;
                CurrentPage = currentPage.ToString();
            }
            PageChangedFunc();
        }
        //尾页
        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = GetIntVal(TotalPage);
            CurrentPage = currentPage.ToString();
            PageChangedFunc();
        }

        /// <summary>
        /// 转到第n页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = GetIntVal(NavigationPage);
            CurrentPage = currentPage.ToString();
            PageChangedFunc();
        }

        private int GetIntVal(string val)
        {
            int temp = 0;
            if (!int
                .TryParse(val, out temp))
            {
                temp = 1;
            }
            return temp;
        }
        /// <summary>
        /// 当当前页值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTotalPageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            //MyButton hsb = (MyButton)sender;
            //  SetValue(PagePicker.CurrentPageProperty, "1");
            // SetFunc();
            //Image image = hsb.tehImage;
            //CurrentPage = "1";
            //image.Source = new BitmapImage((Uri)e.NewValue);
        }
        /// <summary>
        /// 当当前页值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCurrentPageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            //MyButton hsb = (MyButton)sender;
            //  SetValue(PagePicker.CurrentPageProperty, "1");
            // ShowMsg("event");
            //Image image = hsb.tehImage;
            //CurrentPage = "1";
            //image.Source = new BitmapImage((Uri)e.NewValue);
        }

        private static void OnNavigationPageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
