using app.Domain.Exceptions;
using app.Domain.Extensions;
using app.Domain.Models.Authentication;
using app.Domain.Models.Filters;
using app.Domain.Repositories;
using app.Domain.Resources;
using app.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace app.Application
{
    public class UserService : IUserService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;

        public UserService(IOptions<JwtOptions> options, IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _jwtOptions = options.Value;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<UserAuthenticated> Login(User user)
        {
            try
            {
                var userAux = GetByLogin(user.Login);

                if (userAux == null)
                {
                    throw new DomainException(string.Format(CustomMessages.UserNotExists, user.Login));
                }

                if (!VerifyPassword(user.Password, userAux.Password))
                {
                    return new UserAuthenticated()
                    {
                        Authenticated = false,
                        Acesstoken = "",
                        Created = DateTime.Now,
                        Expiration = DateTime.Now.AddMinutes(1440), // 24h
                        Message = CustomMessages.NotAuthenticated,
                        SessionId = null
                    };
                }

                
                var guid = Guid.NewGuid().ToString();
                userAux.SessionId = guid;

                var tokenAplication = GenerateToken(userAux);
                userAux.Profile = null; // Tracking error because of circular dependency caused by GenerateToken method

                await _userRepository.Update(userAux);

                return new UserAuthenticated()
                {
                    Authenticated = true,
                    Acesstoken = tokenAplication,
                    Created = DateTime.Now,
                    Expiration = DateTime.Now.AddMinutes(1440), // 24h
                    Message = CustomMessages.Authenticated,
                    SessionId = guid
                };
            }

            catch (Exception ex)
            {
                throw new SecurityException(ex.Message);
            }
        }

        public PagedList<User> GetAll(UserSearch search)
        {
            search.ThrowIfNotValid();
            return _userRepository.GetAll(search);
        }

        public User GetByLogin(string login)
        {
            return _userRepository.GetByLogin(login);
        }

        public User GetById(long id)
        {
            return _userRepository.GetById(id);
        }

        public async Task Add(User user)
        {
            user.ThrowIfNotValid();
            var userAlreadyExsists = GetByLogin(user.Login);
            var profile = _profileRepository.GetById(user.ProfileId);
            user.Password = HashPassword(user.Password);

            if (userAlreadyExsists != null)
            {
                throw new DomainException(string.Format(CustomMessages.UserAlreadyExists, user.Login));
            }

            if (profile == null)
            {
                throw new DomainException(CustomMessages.ProfileIdNotExists);
            }

            await _userRepository.Add(user);
        }

        public async Task Update(User user)
        {
            user.ThrowIfNotValid();
            var existingUser = GetById(user.Id);
            var loginAlreadyExsists = GetByLogin(user.Login);
            var profile = _profileRepository.GetById(user.ProfileId);

            if (loginAlreadyExsists != null && loginAlreadyExsists.Id != user.Id)
            {
                throw new DomainException(string.Format(CustomMessages.UserAlreadyExists, user.Login));
            }

            if (existingUser == null)
            {
                throw new DomainException(CustomMessages.UserIdNotExists);
            }

            if (profile == null)
            {
                throw new DomainException(CustomMessages.ProfileIdNotExists);
            }

            user.Password = existingUser.Password;

            await _userRepository.Update(user);
        }

        public async Task ChangePassword(User user)
        {
            var userExisting = GetById(user.Id);

            if (userExisting == null)
            {
                throw new DomainException(CustomMessages.UserIdNotExists);
            }

            if (!VerifyPassword(user.Password, userExisting.Password))
            {
                throw new DomainException(CustomMessages.IncorrectPassword);
            }

            userExisting.Profile = null; // Tracking error because of circular dependency caused by GetById method
            userExisting.Password = HashPassword(user.NewPassword);

            await _userRepository.Update(userExisting);
        }

        public void DeleteById(long id)
        {
            var user = GetById(id);

            if(user == null)
            {
                throw new DomainException(CustomMessages.UserIdNotExists);
            }

            _userRepository.DeleteById(id);
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string GenerateToken(User user)
        {
            var symmetricKey = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Login));
            identity.AddClaim(new Claim("Login", user.Login));
            identity.AddClaim(new Claim("Name", user.Name));
            identity.AddClaim(new Claim("SessionId", user.SessionId));

            if (user.Profile.Permissions != null && user.Profile.Permissions.Any())
            {
                identity.AddClaims(user.Profile.Permissions.Select(c => new Claim(ClaimTypes.Role, c.Name)).ToList());
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(symmetricKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(240),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtOptions.Issuer
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}