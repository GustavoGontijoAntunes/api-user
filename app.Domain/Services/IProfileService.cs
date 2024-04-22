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
        Task Add(Profile profile);
        Task Update(Profile profile);
        void DeleteById(long id);
    }
}