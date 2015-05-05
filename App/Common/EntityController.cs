using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace App.Common
{
    public abstract class EntityController<TEntity> : ApiController 
        where TEntity : class
    {
        protected IQueryable<TEntity> Query()
        {
            return _repository.Query<TEntity>();
        }

        protected TEntity Get(params object[] keyValues)
        {
            return _repository.Get<TEntity>(keyValues);
        }

        protected async Task<TEntity> GetAsync(params object[] keyValues)
        {
            return await _repository.GetAsync<TEntity>(keyValues);
        }

        protected async Task<TEntity> GetAsync(CancellationToken token, params object[] keyValues)
        {
            return await _repository.GetAsync<TEntity>(token, keyValues);
        }

        protected TEntity Add(TEntity entity)
        {
            return _repository.Add(entity);
        }

        protected TEntity Update(TEntity entity)
        {
            return _repository.Update(entity);
        }

        protected void Remove(params object[] keyValues)
        {
            _repository.Remove<TEntity>(keyValues);
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

        protected EntityController(IAsyncRepository repository)
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