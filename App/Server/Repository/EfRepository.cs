using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Repository
{
    public class EfRepository<TContext> : IAsyncRepository where TContext : DbContext, new()
    {
        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }

        public T Get<T>(params object[] keyValues) where T : class
        {
            return Context.Set<T>().Find(keyValues);
        }

        public async Task<T> GetAsync<T>(params object[] keyValues) where T : class
        {
            return await _context.Set<T>().FindAsync(keyValues);
        }

        public async Task<T> GetAsync<T>(CancellationToken cancellationToken, params object[] keyValues) where T : class
        {
            return await _context.Set<T>().FindAsync(cancellationToken, keyValues);
        }

        public T Add<T>(T entity) where T : class
        {
            return Context.Set<T>().Add(entity);
        }

        public T Update<T>(T entity) where T : class
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Remove<T>(params object[] keyValues) where T : class
        {
            var entity = Get<T>(keyValues);
            Context.Set<T>().Remove(entity);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected TContext Context { get { return _context; }}

        public EfRepository()
        {
            _context = new TContext();
        }

        private readonly TContext _context;
    }
}