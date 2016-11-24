using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Language;

namespace Rld.Acs.WpfApplication
{
    public class DictionaryManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysDictionaryRepository _sysDictionaryRepo = NinjectBinder.GetRepository<ISysDictionaryRepository>();

        private static DictionaryManager _instance = null;

        public static DictionaryManager GetInstance()
        {
            return _instance;
        }

        public static void Initialize()
        {
            Log.Info("Initializing DictionaryManager...");
            _instance = new DictionaryManager();

            Log.Info("Initializing DictionaryManager Finish...");
        }

        private DictionaryManager()
        {
        }

        public List<SysDictionary> GetDictionaryItemsByTypeId(Int32 typeId)
        {
            return _sysDictionaryRepo.Query(new Hashtable())
                   .FindAll(x => x.Status == GeneralStatus.Enabled && x.Level == (int)DictionaryLevel.TypeItemsLevel && x.TypeID == typeId && x.LanguageID == (int)LanguageManager.CurrentLanguage)
                   .ToList();
        }

        public List<SysDictionary> GetAllTypeHeaders()
        {
            return _sysDictionaryRepo.Query(new Hashtable())
                   .FindAll(x => x.Status == GeneralStatus.Enabled && x.Level == (int)DictionaryLevel.TypeHeaderLevel && x.LanguageID == (int)LanguageManager.CurrentLanguage)
                   .ToList();
        }
    }
}
