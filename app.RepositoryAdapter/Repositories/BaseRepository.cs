using app.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        protected DbSet<TEntity> Repository;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            Repository = context.Set<TEntity>();
        }

        public bool SaveChanges(string? userName = null)
        {
            return _context.SaveChanges(userName) > 0;
        }

        public async Task<bool> SaveChangesAsync(string? userId = null)
        {
            return (await _context.SaveChangesAsync(userId)) > 0;
        }

        public void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Repository.Update(entity);
        }
    }
}