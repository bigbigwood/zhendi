using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class SysDictionaryPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysDictionaryRepository _sysDictionaryRepo = NinjectBinder.GetRepository<ISysDictionaryRepository>();

        public String Keyword { get; set; }
        public SysDictionaryViewModel SelectedSysDictionaryViewModel { get; set; }
        public ObservableCollection<SysDictionaryViewModel> SysDictionaryViewModels { get; set; }
        public Int32 SelectedTypeHeader { get; set; }
        public List<SysDictionary> TypeHeadersDict { get; set; }

        public RelayCommand QueryCommand { get; set; }

        private async void QueryCommandFunc()
        {
            await Task.Run(() =>
            {
                int totalCount = 0;
                var pageIndex = 1;
                SysDictionaryViewModels = QueryData(pageIndex, PageSize, out totalCount);
                if (totalCount % PageSize == 0)
                {
                    TotalPage = (totalCount / PageSize).ToString();
                }
                else
                {
                    TotalPage = ((totalCount / PageSize) + 1).ToString();
                }

                RaisePropertyChanged(null);
            });
        }

        #region 分页相关属性

        /// <summary>
        /// 分页查询命令
        /// </summary>
        private async void NextPageSearchCommandFunc()
        {
            await Task.Run(() =>
            {
                int totalCount = 0;
                var pageIndex = Convert.ToInt32(CurrentPage);
                SysDictionaryViewModels = QueryData(pageIndex, PageSize, out totalCount);
                if (totalCount % PageSize == 0)
                {
                    TotalPage = (totalCount / PageSize).ToString();
                }
                else
                {
                    TotalPage = ((totalCount / PageSize) + 1).ToString();
                }

                RaisePropertyChanged(null);
            });
        }
        private string _totalPage = string.Empty;


        /// <summary>
        /// 总页数
        /// </summary>
        public string TotalPage
        {
            get { return _totalPage; }
            set
            {
                _totalPage = value;
                RaisePropertyChanged();
            }
        }
        private string _navigationPage = string.Empty;

        public string NavigationPage
        {
            get { return _navigationPage; }
            set
            {
                _navigationPage = value;
                RaisePropertyChanged();
            }
        }

        private string _currentPage = "1";
        /// <summary>
        /// 当前页
        /// </summary>
        public string CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        private int _pageSize = 30;
        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged();
            }
        }
        private int _pageIndex;
        private int _totalCount;
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;

                RaisePropertyChanged();
            }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;

                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 分页管理
        /// </summary>
        public RelayCommand NextPageSearchCommand { get; set; }

        #endregion

        public SysDictionaryPageViewModel()
        {
            QueryCommand = new RelayCommand(QueryCommandFunc);
            NextPageSearchCommand = new RelayCommand(NextPageSearchCommandFunc);

            SysDictionaryViewModels = new ObservableCollection<SysDictionaryViewModel>();
            TypeHeadersDict = new List<SysDictionary>() {new SysDictionary() {ItemID = -1, ItemValue = ""}};
            TypeHeadersDict.AddRange(DictionaryManager.GetInstance().GetAllTypeHeaders());
            SelectedTypeHeader = -1;
        }


        private ObservableCollection<SysDictionaryViewModel> QueryData(int pageIndex, int pageSize, out int totalCount)
        {
            Int32 pageStart = pageSize * (pageIndex - 1) + 1;
            Int32 pageEnd = pageSize * pageIndex;

            var conditions = GetConditions();
            conditions.Add("PageStart", pageStart);
            conditions.Add("PageEnd", pageEnd);

            var paninationResult = _sysDictionaryRepo.QueryPage(conditions);
            totalCount = paninationResult.TotalCount;
            var logVM = paninationResult.Entities.Select(AutoMapper.Mapper.Map<SysDictionaryViewModel>);

            SysDictionaryViewModels = new ObservableCollection<SysDictionaryViewModel>(logVM);
            return SysDictionaryViewModels;
        }

        private Hashtable GetConditions()
        {
            var conditions = new Hashtable()
            {
                {"Level", (int)DictionaryLevel.TypeItemsLevel},
            };

            if (!string.IsNullOrWhiteSpace(Keyword))
                conditions.Add("Keyword", Keyword);

            if (SelectedTypeHeader != -1)
                conditions.Add("TypeID", SelectedTypeHeader);

            return conditions;
        }
    }
}
