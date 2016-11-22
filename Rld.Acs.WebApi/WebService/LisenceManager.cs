using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;

namespace Rld.Acs.WebApi.WebService
{
    public class LisenceManager
    {
        private static object lockObj = new object();
        private static List<string> _onlineLisences = null;
        private static int configId = 0;

        private static List<string> onlineLisences
        {
            get
            {
                if (_onlineLisences == null)
                    _onlineLisences = GetOnlineLisences();
                return _onlineLisences;
            }
        }

        public bool Register(string id)
        {
            var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
            lock (lockObj)
            {
                if (onlineLisences.Contains(id)) return true;
                if (onlineLisences.Count >= GetLisenceMaxCount()) return false;

                onlineLisences.Add(id);
                repo.Update(new SysConfig() { ID = configId, Name = ConstStrings.LisenceOnlineCount, Value = string.Join(",", onlineLisences) });
            }

            return true;
        }

        public bool Unregister(string id)
        {
            var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
            lock (lockObj)
            {
                if (onlineLisences.Contains(id))
                {
                    lock (lockObj)
                    {
                        onlineLisences.Remove(id);
                        repo.Update(new SysConfig() { ID = configId, Name = ConstStrings.LisenceOnlineCount, Value = string.Join(",", onlineLisences) });
                    }
                }
            }

            return true;
        }

        private int GetLisenceMaxCount()
        {
            return ConfigurationManager.AppSettings.Get("LisenceMaxCount").ToInt32();
        }

        private static List<string> GetOnlineLisences()
        {
            var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
            var config = repo.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.LisenceOnlineCount);
            configId = config.ID;

            return config.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}