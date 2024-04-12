using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;
using app.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter.Repositories
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public PagedList<Profile> GetAll(ProfileSearch search)
        {
            var query = Repository
                .Include(x => x.Permissions).AsQueryable();

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            return PagedList<Profile>.ToPagedList(query, search.PageIndex, search.PageSize);
        }

        public Profile GetByName(string name)
        {
            var query = Repository
                .Include(x => x.Permissions).AsQueryable();

            return query.FirstOrDefault(x => x.Name == name);
        }

        public Profile GetById(long id)
        {
            var query = Repository
                .Include(x => x.Permissions).AsQueryable();

            return query.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(long id)
        {
            var profile = Repository.AsNoTracking().AsQueryable().FirstOrDefault(x => x.Id == id);
            Repository.Remove(profile);
        }

        public bool IsUsedInSomeUser(long id)
        {
            var queryUser = _context.Set<User>().AsQueryable();

            return queryUser.Where(x => x.ProfileId == id).Any();
        }
    }
}