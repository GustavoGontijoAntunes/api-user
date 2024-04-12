namespace app.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool SaveChanges(string? userId = null);
        Task<bool> SaveChangesAsync(string? userId = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
    }
}