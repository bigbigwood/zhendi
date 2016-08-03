﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    [KnownType("KnownTypes")]
    public class CredentialService : ServiceBase
    {
        [DataMember]
        public Boolean UseForDuress { get; set; }

        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(CredentialService).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }
}