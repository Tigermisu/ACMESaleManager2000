using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    interface IService<TDomainObject>
    {
        List<TDomainObject> GetAll();
    }
}
