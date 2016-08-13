using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Rld.Acs.Unility.Serialization
{
    public static class DataContractSerializationHelper
    {
        public static String Serialize<T>(T instance)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, instance);
                ms.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static T Deserialize<T>(string data)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                T response = (T)serializer.ReadObject(ms);
                return response;
            }
        }

    }
}
