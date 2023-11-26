using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Repository
{
    public interface IRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : DbContext
    {
        #region Query & List
        IQueryable<TEntity> QueryAll(params Expression<Func<TEntity, object>>[] eagerProperties);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

        Task<List<TEntity>> ListAllAsync(params Expression<Func<TEntity, object>>[] eagerProperties);

        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

        #endregion

        #region Find & First Or Default
        Task<TEntity> FindAsync(params object[] pk);

        Task<TEntity> FindDetachedAsync(long id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

        #endregion

        #region All & Any 
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Remove
        void Remove(long pk);
        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<long> pks);
        void RemoveRange(IEnumerable<TEntity> entities);
        #endregion

        #region Remove And Save

        Task<int> RemoveAndSaveAsync(long pk);
        Task<int> RemoveAndSaveAsync(TEntity entity);

        Task<int> RemoveRangeAndSaveAsync(IEnumerable<long> pks);
        Task<int> RemoveRangeAndSaveAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Modify
        void Modify(TEntity entity);
        void ModifyRange(List<TEntity> list);
        #endregion

        #region Modify And Save

        Task<int> ModifyAndSaveAsync(TEntity entity);
        Task<int> ModifyRangeAndSaveAsync(List<TEntity> list);
        #endregion

        #region Add & Insert
        void Add(TEntity entity);

        void AddRange(List<TEntity> entites);

        TEntity AddEntity(TEntity entity);

        #endregion

        #region Add & Save

        Task AddAndSaveAsync(TEntity entity);

        Task<int> AddRangeAndSaveAsync(List<TEntity> entites);

        #endregion

        #region Count
        int Count(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

        #endregion

        #region Save

        Task<int> SaveAsyc();

        #endregion
    }
}
