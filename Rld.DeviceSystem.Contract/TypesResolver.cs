using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Message;

namespace Rld.DeviceSystem.Contract
{
    public static class TypesResolver
    {
        private static List<Type> knownTypes = null;
        private static List<Type> knownResources = null;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            if (knownTypes != null)
                return knownTypes;

            knownTypes = new List<Type>();

            //Type typeofRequest = typeof(RequestBase);
            //Type typeOfResponse = typeof(ResponseBase);

            foreach (var type in typeof(Declarations).Assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(DataContractAttribute), true).Any())
                    knownTypes.Add(type);
                else if (type.GetCustomAttributes(typeof(MessageContractAttribute), true).Any())
                    knownTypes.Add(type);
                else
                    Console.WriteLine("Unknwon type:" + type);


            }
            return knownTypes;
        }
    }
}
