using app.Domain.Models;

namespace app.Domain.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        IEnumerable<Permission> GetAll();
        Permission GetByName(string name);
        Permission GetById(long id);
        Task AddOrUpdateRange(List<Permission> permissions);
        void DeleteById(long id);
        List<Permission> GetPermissionsByProfileId(long profileId);
        Task AddOrUpdatePermissionsToProfile(List<ProfilePermission> profilePermissions);
        void DeleteProfilePermissionsByProfileId(long profileId);
        bool IsUsedInSomeProfile(long id);
    }
}