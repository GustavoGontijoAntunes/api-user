using app.Domain.Extensions;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;

namespace app.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool Login(string login, string password);
        PagedList<User> GetAll(UserSearch search);
        User GetByLogin(string login);
        User GetById(long id);
        void DeleteById(long id);
    }
}