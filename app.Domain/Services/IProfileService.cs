using app.Domain.Extensions;
using app.Domain.Models.Filters;
using app.Domain.Models;

namespace app.Domain.Services
{
    public interface IProfileService
    {
        PagedList<Profile> GetAll(ProfileSearch search);
        Profile GetByName(string name);
        Profile GetById(long id);
        void Add(Profile profile, string userName);
        void Update(Profile profile, string userName);
        void DeleteById(long id, string userName);
    }
}