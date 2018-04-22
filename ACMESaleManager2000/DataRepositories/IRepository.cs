using ACMESaleManager2000.DataEntities;
using System.Collections.Generic;

namespace ACMESaleManager2000.DataRepositories
{
    public interface IRepository<TDomainObject>
    {
        List<TDomainObject> GetAll();

        TDomainObject GetEntity(int Id);

        bool SaveModifiedEntity(IEntity entity);

        bool EntityExists(int Id);

        bool DeleteEntity(int Id);

        void CreateEntity(TDomainObject entity);
    }
}