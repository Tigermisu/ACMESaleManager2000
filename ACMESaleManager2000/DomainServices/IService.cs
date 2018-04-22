using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public interface IService<TDomainObject>
    {
        List<TDomainObject> GetAll();

        bool EntityExists(int Id);

        bool DeleteEntity(int Id);
    }
}
