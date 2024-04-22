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

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task Add(TEntity entity)
        {
            Repository.Add(entity);
            await SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            Repository.Update(entity);
            await SaveChangesAsync();
        }
    }
}