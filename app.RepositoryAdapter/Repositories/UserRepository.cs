using app.Domain.Extensions;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;
using app.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public bool Login(string login, string password)
        {
            var query = Repository.AsQueryable();
            var result = query.Where(x => x.Login == login && x.Password == password);

            return result.Any();
        }

        public PagedList<User> GetAll(UserSearch search)
        {
            var query = Repository
                .Include(x => x.Profile).ThenInclude(y => y.Permissions).AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            if (!string.IsNullOrEmpty(search.Login))
            {
                query = query.Where(x => x.Login.Contains(search.Login));
            }

            if (search.ProfileId > 0)
            {
                query = query.Where(x => x.ProfileId == search.ProfileId);
            }

            return PagedList<User>.ToPagedList(query, search.PageIndex, search.PageSize);
        }

        public User GetByLogin(string login)
        {
            var query = Repository.AsNoTracking()
                .Include(x => x.Profile).ThenInclude(y => y.Permissions).AsQueryable();

            return query.FirstOrDefault(x => x.Login == login);
        }

        public User GetById(long id)
        {
            var query = Repository.AsNoTracking()
                .Include(x => x.Profile).ThenInclude(y => y.Permissions).AsQueryable();

            return query.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(long id)
        {
            var user = Repository.AsNoTracking().AsQueryable().FirstOrDefault(x => x.Id == id);

            Repository.Remove(user);
            SaveChanges();
        }
    }
}