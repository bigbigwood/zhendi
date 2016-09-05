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
    public class FloorPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IFloorRepository _floorRepo = NinjectBinder.GetRepository<IFloorRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public ObservableCollection<FloorViewModel> FloorViewModels { get; set; }
        public FloorViewModel SelectedFloorViewModel { get; set; }

        public FloorPageViewModel()
        {
            AddCmd = new AuthCommand(Add);
            ModifyCmd = new AuthCommand(Modify);
            DeleteCmd = new AuthCommand(ShowDeletionQuestion);

            var operators = _floorRepo.Query(new Hashtable());
            var vms = operators.Select(AutoMapper.Mapper.Map<FloorViewModel>);
            FloorViewModels = new ObservableCollection<FloorViewModel>(vms);
        }

        private void Add()
        {
            try
            {
                var viewModel = AutoMapper.Mapper.Map<FloorViewModel>(new Floor());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.FloorView_Open);

                if (viewModel.FloorID != 0)
                {
                    FloorViewModels.Add(viewModel);
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
                if (SelectedFloorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.FloorPage_ShowNotification);
                    return;
                }

                SelectedFloorViewModel.InitDoorListBox();
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedFloorViewModel

                }, Tokens.FloorView_Open);

                RaisePropertyChanged(null);
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
                if (SelectedFloorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.FloorPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除:{0}吗？", SelectedFloorViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, Delete), Tokens.FloorPage_ShowQuestion);
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
                    _floorRepo.Delete(SelectedFloorViewModel.FloorID);
                    message = "删除成功!";

                    FloorViewModels.Remove(SelectedFloorViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.FloorPage_ShowNotification);
            });
        }
    }
}
