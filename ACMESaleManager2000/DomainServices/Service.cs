﻿using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DataRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    abstract public class Service<TDomainObject> : IService<TDomainObject>
    {
        protected IRepository<TDomainObject> _repository { get; }

        public Service(IRepository<TDomainObject> repository) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<TDomainObject> GetAll() {
            return _repository.GetAll();
        }

        public TDomainObject GetEntity(int Id) {
            return _repository.GetEntity(Id);
        }

        public bool SaveModifiedEntity(IEntity entity) {
            return _repository.SaveModifiedEntity(entity);
        }

        public bool EntityExists(int Id) {
            return _repository.EntityExists(Id);
        }

        public bool DeleteEntity(int Id) {
            return _repository.DeleteEntity(Id);
        }

        public void CreateEntity(TDomainObject entity) {
            _repository.CreateEntity(entity);
        }
    }
}
