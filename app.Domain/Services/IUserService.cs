using app.Domain.Extensions;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;

namespace app.Domain.Services
{
    public interface IUserService
    {
        UserAuthenticated Login(User user, string userName);
        PagedList<User> GetAll(UserSearch search);
        User GetByLogin(string login);
        User GetById(long id);
        void Add(User user, string userName);
        void Update(User user, string userName);
        void ChangePassword(User user, string userName);
        void DeleteById(long id, string userName);
    }
}