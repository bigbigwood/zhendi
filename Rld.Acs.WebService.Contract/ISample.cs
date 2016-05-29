using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Rld.Acs.WebService.Contract
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface ISample
    {
        [OperationContract]
        void DoSomething();
    }
}
