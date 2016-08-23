using System;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using log4net;
using Rld.Acs.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Messages;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Repository;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysDictionaryViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysDictionaryRepository _sysDictionaryRepo = NinjectBinder.GetRepository<ISysDictionaryRepository>();

        public virtual Int32 DictionaryID { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32? TypeID { get; set; }
        public virtual String TypeName { get; set; }
        public virtual Int32? ParentID { get; set; }
        public virtual Int32? LanguageID { get; set; }
        public virtual Int32? Level { get; set; }
        public virtual Int32? ItemID { get; set; }
        public virtual String ItemProperty { get; set; }
        public virtual String ItemValue { get; set; }
        public virtual String Description { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public List<SysDictionary> TypeHeadersDict { get; set; }

        public SysDictionaryViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            TypeHeadersDict = DictionaryManager.GetInstance().GetAllTypeHeaders();
        }


        private void Save()
        {
            string message = "";
            try
            {
                var selected = TypeHeadersDict.FirstOrDefault(d => d.TypeID == TypeID);
                Name = selected.Name;
                TypeName = selected.TypeName;
                ParentID = 0;
                Level = 2;
                Status = GeneralStatus.Enabled;

                if (DictionaryID == 0)
                {
                    CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CreateDate = DateTime.Now;

                    var coreModel = Mapper.Map<SysDictionary>(this);
                    coreModel = _sysDictionaryRepo.Insert(coreModel);

                    DictionaryID = coreModel.DictionaryID;
                    message = "增加数据项成功!";
                }
                else
                {
                    UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    UpdateDate = DateTime.Now;

                    var coreModel = Mapper.Map<SysDictionary>(this);
                   _sysDictionaryRepo.Update(coreModel);
                   message = "修改数据项成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update device fails.", ex);
                message = "保存数据项失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseSysDictionaryView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.SysDictionaryView_ShowNotification);
        }
    }
}
