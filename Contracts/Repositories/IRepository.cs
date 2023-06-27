using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IRepository
    {
        /// <summary>
        /// Remove Entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void RemoveEntities<T>(IEnumerable<T> entities) where T : class, new();

        /// <summary>
        /// Edit entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void EditEntities<T>(IEnumerable<T> entities) where T : class, new();

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void EditEntity<T>(T entity) where T : class, new();

        /// <summary>
        /// Save entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void Save<T>(T entity, bool isEdit = true) where T : class, new();

        /// <summary>
        /// SaveList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="isEdit"></param>
        void SaveList<T>(IEnumerable<T> entityList, bool isEdit = true) where T : class, new();

        /// <summary>
        /// Get single element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class, new();


        /// <summary>
        /// Get single element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new();

        /// <summary>
        /// Get List async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new();

        /// <summary>
        /// Get Queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> predicate) where T : class, new();


        /// <summary>
        /// Add entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        void AddEntity<T>(T enity) where T : class, new();

        /// <summary>
        /// Exists 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        /// <returns></returns>
        bool Exists<T>(Func<T,bool> predicate) where T : class, new();

        /// <summary>
        /// Exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        /// <returns></returns>
        bool Exists<T>() where T : class, new();

        /// <summary>
        /// Count 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        /// <returns></returns>
        int Count<T>(Func<T, bool> predicate) where T : class, new();

        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enity"></param>
        /// <returns></returns>
        int Count<T>() where T : class, new();


        /// <summary>
        /// Save context
        /// </summary>
        /// <returns></returns>
        Task Save();


        /// <summary>
        /// Get transaction
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();


    }
}
