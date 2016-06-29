using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Rld.Acs.WebApi.Framework
{
    public static class HttpRequestMessageExtension
    {
        public static Hashtable GetQueryNameValueHashtable(this HttpRequestMessage self)
        {
            var conditions = new Hashtable();
            var allUrlKeyValues = self.GetQueryNameValuePairs();
            allUrlKeyValues.ToList().ForEach(p => conditions.Add(p.Key, p.Value));

            return conditions;
        }
    }
}