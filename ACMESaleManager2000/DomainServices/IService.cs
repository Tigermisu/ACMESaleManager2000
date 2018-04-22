using ACMESaleManager2000.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public interface IService<TDomainObject>
    {
        List<TDomainObject> GetAll();

        TDomainObject GetEntity(int Id); 

        void CreateEntity(TDomainObject entity);

        bool SaveModifiedEntity(IEntity entity);

        bool EntityExists(int Id);

        bool DeleteEntity(int Id);
    }
}
