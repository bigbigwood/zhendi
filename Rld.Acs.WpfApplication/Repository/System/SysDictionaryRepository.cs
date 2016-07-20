using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class SysDictionaryRepository : BaseRepository<SysDictionary, int>, ISysDictionaryRepository
    {
        public SysDictionaryRepository()
        {
            RelevantUri = "/api/SysDictionarys";
        }

        public override bool Update(SysDictionary sysDictionary)
        {
            return Update(sysDictionary, sysDictionary.DictionaryID);
        }
    }
}
