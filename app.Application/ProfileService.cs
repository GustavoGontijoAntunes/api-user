using app.Domain.Exceptions;
using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Filters;
using app.Domain.Repositories;
using app.Domain.Resources;
using app.Domain.Services;

namespace app.Application
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public PagedList<Profile> GetAll(ProfileSearch search)
        {
            search.ThrowIfNotValid();
            return _profileRepository.GetAll(search);
        }

        public Profile GetByName(string name)
        {
            return _profileRepository.GetByName(name);
        }

        public Profile GetById(long id)
        {
            return _profileRepository.GetById(id);
        }

        public void Add(Profile profile, string userName)
        {
            profile.ThrowIfNotValid();
            var profileAlreadyExsists = GetByName(profile.Name);

            if (profileAlreadyExsists != null)
            {
                throw new DomainException(string.Format(CustomMessages.ProfileAlreadyExists, profile.Name));
            }

            _profileRepository.Add(profile);
            SaveChanges(userName);
        }

        public void Update(Profile profile, string userName)
        {
            profile.ThrowIfNotValid();
            var existingProfile = GetById(profile.Id);
            var profileAlreadyExsists = GetByName(profile.Name);

            if (profileAlreadyExsists != null && profileAlreadyExsists.Id != profile.Id)
            {
                throw new DomainException(string.Format(CustomMessages.ProfileAlreadyExists, profile.Name));
            }

            if (existingProfile == null)
            {
                throw new DomainException(CustomMessages.ProfileIdNotExists);
            }

            _profileRepository.Update(profile);
            SaveChanges(userName);
        }

        public void DeleteById(long id, string userName)
        {
            var profile = GetById(id);
            var isAlreadyUsed = IsUsedInSomeUser(id);

            if (profile == null)
            {
                throw new DomainException(CustomMessages.PermissionIdNotExists);
            }

            if (isAlreadyUsed)
            {
                throw new DomainException(CustomMessages.ProfileCantBeDeleted);
            }

            _profileRepository.DeleteById(id);
            SaveChanges(userName);
        }

        private void SaveChanges(string userName)
        {
            _profileRepository.SaveChanges(userName);
        }

        private bool IsUsedInSomeUser(long id)
        {
            return _profileRepository.IsUsedInSomeUser(id);
        }
    }
}