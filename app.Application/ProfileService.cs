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

        public async Task Add(Profile profile)
        {
            profile.ThrowIfNotValid();
            var profileAlreadyExsists = GetByName(profile.Name);

            if (profileAlreadyExsists != null)
            {
                throw new DomainException(string.Format(CustomMessages.ProfileAlreadyExists, profile.Name));
            }

            await _profileRepository.Add(profile);
        }

        public async Task Update(Profile profile)
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

            await _profileRepository.Update(profile);
        }

        public void DeleteById(long id)
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
        }

        private bool IsUsedInSomeUser(long id)
        {
            return _profileRepository.IsUsedInSomeUser(id);
        }
    }
}