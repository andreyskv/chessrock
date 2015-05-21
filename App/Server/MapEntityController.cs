using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using App.Repository;

namespace App.Common
{
    public abstract class MapEntityController<TEntity, TData> : ApiController 
        where TEntity : class
        where TData : class 
    {
        protected IQueryable<TData> Query()
        {
            return _repository.Query<TEntity>().Project().To<TData>();
        }

        protected IQueryable<TData> Query(Expression<Func<TEntity, bool>> entityPredicate)
        {
            return _repository.Query<TEntity>()
                .Where(entityPredicate)
                .Project()
                .To<TData>();
        }

        protected TData Get(params object[] keyValues)
        {
            var entity = _repository.Get<TEntity>(keyValues);
            return GetDataObject(entity);
        }

        protected async Task<TData> GetAsync(params object[] keyValues)
        {
            var entity = await _repository.GetAsync<TEntity>(keyValues);
            return GetDataObject(entity);
        }

        protected async Task<TData> GetAsync(CancellationToken token, params object[] keyValues)
        {
            var entity = await _repository.GetAsync<TEntity>(token, keyValues);
            return GetDataObject(entity);
        }

        protected async Task<TData> Add(TData data)
        {
            var entity = GetEntityObject(data);
            entity = _repository.Add(entity);
            await _repository.SaveChangesAsync();
            return GetDataObject(entity);
        }

        protected async Task<TData> Update(TData data)
        {
            var entity = GetEntityObject(data);
            entity = _repository.Update(entity);
            await _repository.SaveChangesAsync();
            return GetDataObject(entity);
        }

        protected async Task Remove(params object[] keyValues)
        {
            _repository.Remove<TEntity>(keyValues);
            await _repository.SaveChangesAsync();
        }

        protected void SaveChanges()
        {
            _repository.SaveChanges();
        }

        protected async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }

        protected async Task SaveChangesAsync(CancellationToken token)
        {
            await _repository.SaveChangesAsync(token);
        }

        protected IAsyncRepository Repository
        {
            get { return _repository; }
        }

        protected TData GetDataObject(TEntity entity)
        {
            return Mapper.Map<TEntity, TData>(entity);
        }

        protected TEntity GetEntityObject(TData data)
        {
            return Mapper.Map<TData, TEntity>(data);
        }

        protected MapEntityController(IAsyncRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }

        private readonly IAsyncRepository _repository;
    }
}