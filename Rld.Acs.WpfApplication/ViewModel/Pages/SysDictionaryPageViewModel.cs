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
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;
using Rld.Acs.WpfApplication.Models.Messages;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

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

        public List<SysDictionary> TypeHeadersDict
        {
            get
            {
                var dict = new List<SysDictionary>() { new SysDictionary() { ItemID = -1, ItemValue = "" } };
                dict.AddRange(DictionaryManager.GetInstance().GetAllTypeHeaders());
                return dict;
            }
        }
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public RelayCommand QueryCommand { get; set; }

        #region 分页相关属性

        private async void QueryCommandFunc()
        {
            await Task.Run(() =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            });
        }


        /// <summary>
        /// 分页查询命令
        /// </summary>
        private async void NextPageSearchCommandFunc()
        {
            await Task.Run(() =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
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
            QueryCommand = new AuthCommand(QueryCommandFunc);
            NextPageSearchCommand = new AuthCommand(NextPageSearchCommandFunc);
            AddCmd = new AuthCommand(AddItem);
            ModifyCmd = new AuthCommand(ModifyItem);
            DeleteCmd = new AuthCommand(DeleteItem);

            SysDictionaryViewModels = new ObservableCollection<SysDictionaryViewModel>();
            SelectedTypeHeader = -1;
        }


        private void AddItem()
        {
            try
            {
                var viewModel = new SysDictionaryViewModel();
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenSysDictionaryView);

                if (viewModel.DictionaryID != 0)
                    SysDictionaryViewModels.Add(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyItem()
        {
            try
            {
                if (SelectedSysDictionaryViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择字典项!"), Tokens.SysDictionaryPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedSysDictionaryViewModel

                }, Tokens.OpenSysDictionaryView);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteItem()
        {
            try
            {
                if (SelectedSysDictionaryViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择字典项!"), Tokens.SysDictionaryPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除字典项:{0}吗？", SelectedSysDictionaryViewModel.ItemValue);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDelete), Tokens.SysDictionaryPage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDelete()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _sysDictionaryRepo.Delete(SelectedSysDictionaryViewModel.DictionaryID);
                    message = "删除成功!";

                    SysDictionaryViewModels.Remove(SelectedSysDictionaryViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.SysDictionaryPage_ShowNotification);
            });
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
