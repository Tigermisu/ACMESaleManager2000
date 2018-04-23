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
    public abstract class Repository<TDomainObject, TEntity> : IRepository<TDomainObject> where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected TDomainObject Map(TEntity entity)
        {
            return Mapper.Map<TDomainObject>(entity);
        }

        protected List<TDomainObject> Map(List<TEntity> entities)
        {
            return entities.Select(e => Mapper.Map<TDomainObject>(e)).ToList();
        }

        abstract protected DbSet<TEntity> DbSet { get; }

        public virtual List<TDomainObject> GetAll() {
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

        public TDomainObject GetEntity(int Id)
        {
            return Map(DbSet.SingleOrDefault(m => m.Id == Id));
        }

        public bool SaveModifiedEntity(IEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if (!EntityExists(entity.Id))
                {
                    return false;
                }
                else {
                    throw;
                }
            }

            return true;
        }

        public void CreateEntity(TDomainObject entity)
        {
            DbSet.Add(Mapper.Map<TEntity>(entity));
            _context.SaveChanges();
        }
    }
}
