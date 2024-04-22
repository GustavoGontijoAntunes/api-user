using app.Domain.Extensions;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;

namespace app.Domain.Services
{
    public interface IUserService
    {
        Task<UserAuthenticated> Login(User user);
        PagedList<User> GetAll(UserSearch search);
        User GetByLogin(string login);
        User GetById(long id);
        Task Add(User user);
        Task Update(User user);
        Task ChangePassword(User user);
        void DeleteById(long id);
    }
}