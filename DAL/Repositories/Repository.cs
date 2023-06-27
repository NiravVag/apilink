using Contracts.Repositories;
using DTO.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL.Repositories
{
    public abstract class Repository : ApiCommonData, IRepository
    {
        protected readonly API_DBContext _context = null;

        public Repository(API_DBContext context)
        {
            _context = context;
        }

        public void AddEntity<T>(T enity) where T : class, new()
        {
            _context.Entry<T>(enity).State = EntityState.Added;
        }

        public int Count<T>(Func<T, bool> predicate) where T : class, new()
        {
            return _context.Set<T>().Count(predicate);
        }

        public int Count<T>() where T : class, new()
        {
            return _context.Set<T>().Count();
        }

        public void EditEntities<T>(IEnumerable<T> entities) where T : class, new()
        {
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                    _context.Entry<T>(entity).State = EntityState.Modified;
            }
        }

        public void EditEntity<T>(T entity) where T : class, new()
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }

        public bool Exists<T>(Func<T, bool> predicate) where T : class, new()
        {
            return _context.Set<T>().Any(predicate);
        }

        public bool Exists<T>() where T : class, new()
        {
            return _context.Set<T>().Any();
        }

        public void RemoveEntities<T>(IEnumerable<T> entities) where T : class, new()
        {
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                    _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public void Save<T>(T entity, bool isEdit = true) where T : class, new()
        {
            if (isEdit)
                _context.Entry<T>(entity).State = EntityState.Modified;
            else
                _context.Entry<T>(entity).State = EntityState.Added;

            _context.SaveChanges();

        }

        public void SaveList<T>(IEnumerable<T> entityList, bool isEdit = true) where T : class, new()
        {
            if (isEdit)
            {
                foreach (var entity in entityList)
                    _context.Entry<T>(entity).State = EntityState.Modified;
            }
            else
            {
                foreach (var entity in entityList)
                    _context.Entry<T>(entity).State = EntityState.Added;
            }

            _context.SaveChanges();

        }

        public T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
