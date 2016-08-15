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
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication
{
    public class DictionaryManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysDictionaryRepository _sysDictionaryRepo = NinjectBinder.GetRepository<ISysDictionaryRepository>();
        private IDictionary<int, List<SysDictionary>> dictionary = new Dictionary<int, List<SysDictionary>>();

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
            List<SysDictionary> dic = null;
            if (!dictionary.TryGetValue(typeId, out dic))
            {
                dic = _sysDictionaryRepo.Query(new Hashtable {{"Status", (int) GeneralStatus.Enabled}, {"TypeID", typeId}}).ToList();
                dictionary.Add(typeId, dic);
            }

            return dic;
        }
    }
}
