using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.ViewModel.Views;
using GalaSoft.MvvmLight.Threading;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class SysOperatorPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysOperatorRepository _sysOperatorRepo = NinjectBinder.GetRepository<ISysOperatorRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public ObservableCollection<SysOperatorViewModel> SysOperatorViewModels { get; set; }
        public SysOperatorViewModel SelectedSysOperatorViewModel { get; set; }

        public SysOperatorPageViewModel()
        {
            AddCmd = new AuthCommand(Add);
            ModifyCmd = new AuthCommand(Modify);
            DeleteCmd = new AuthCommand(ShowDeletionQuestion);

            var operators = _sysOperatorRepo.Query(new Hashtable());
            var vms = operators.Select(AutoMapper.Mapper.Map<SysOperatorViewModel>);
            SysOperatorViewModels = new ObservableCollection<SysOperatorViewModel>(vms);
        }

        private void Add()
        {
            try
            {
                var viewModel = AutoMapper.Mapper.Map<SysOperatorViewModel>(new SysOperator());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.SysOperatorView_Open);

                if (viewModel.OperatorID != 0)
                {
                    viewModel.NewPasswordEnabled = false;
                    viewModel.NewPassword1 = null;
                    viewModel.NewPassword2 = null;
                    SysOperatorViewModels.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Modify()
        {
            try
            {
                if (SelectedSysOperatorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.SysOperatorPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedSysOperatorViewModel

                }, Tokens.SysOperatorView_Open);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ShowDeletionQuestion()
        {
            try
            {
                if (SelectedSysOperatorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.SysOperatorPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除:{0}吗？", SelectedSysOperatorViewModel.LoginName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, Delete), Tokens.SysOperatorPage_ShowQuestion);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Delete()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _sysOperatorRepo.Delete(SelectedSysOperatorViewModel.OperatorID);
                    message = "删除成功!";

                    SysOperatorViewModels.Remove(SelectedSysOperatorViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.SysOperatorPage_ShowNotification);
            });
        }
    }
}
