namespace app.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool SaveChanges();
        Task<bool> SaveChangesAsync();
        Task Add(TEntity entity);
        Task Update(TEntity entity);
    }
}