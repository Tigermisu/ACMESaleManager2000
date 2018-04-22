using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    abstract public class Repository<TDomainObject, TEntity> : IRepository<TDomainObject> where TEntity : class, IEntity
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

        abstract protected DbSet<TEntity> DbSet { get; }

        public List<TDomainObject> GetAll() {
            return Map(DbSet.ToList());
        }

        public bool EntityExists(int Id)
        {
            return DbSet.Any(e => e.Id == Id);

        }
        public bool DeleteEntity(int Id) {
            TEntity entity = DbSet.SingleOrDefault(m => m.Id == Id);

            if (entity == null) {
                return false;
            }

            DbSet.Remove(entity);
            _context.SaveChanges();

            return true;
        }
    }
}
