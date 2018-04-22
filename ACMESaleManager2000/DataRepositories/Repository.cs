﻿using ACMESaleManager2000.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    abstract public class Repository<TDomainObject, TEntity> : IRepository<TDomainObject>
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected List<TDomainObject> Map(List<TEntity> entities)
        {
            return entities.Select(e => Mapper.Map<TDomainObject>(e)).ToList();
        }

        public abstract List<TDomainObject> GetAll();

        public abstract bool EntityExists(int Id);
    }
}
