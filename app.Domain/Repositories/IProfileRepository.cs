using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Filters;

namespace app.Domain.Repositories
{
    public interface IProfileRepository : IRepository<Profile>
    {
        PagedList<Profile> GetAll(ProfileSearch search);
        Profile GetByName(string name);
        Profile GetById(long id);
        void DeleteById(long id);
        bool IsUsedInSomeUser(long id);
    }
}