using app.Domain.Models;
using app.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace app.RepositoryAdapter.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Permission> GetAll()
        {
            var query = Repository.AsQueryable();

            return query.ToList();
        }

        public Permission GetByName(string name)
        {
            var query = Repository.AsQueryable();

            return query.FirstOrDefault(x => x.Name == name);
        }

        public Permission GetById(long id)
        {
            var query = Repository.AsQueryable();

            return query.FirstOrDefault(x => x.Id == id);
        }

        public async Task AddOrUpdateRange(List<Permission> permissions)
        {
            Repository.UpdateRange(permissions);
            await SaveChangesAsync();
        }

        public void DeleteById(long id)
        {
            var permission = Repository.AsNoTracking().AsQueryable().FirstOrDefault(x => x.Id == id);

            Repository.Remove(permission);
            SaveChanges();
        }

        public List<Permission> GetPermissionsByProfileId(long profileId)
        {
            var query = _context.Set<ProfilePermission>().AsQueryable();
            var permissions = query.Where(x => x.ProfileId == profileId).Select(y => y.Permission).ToList();

            return permissions;
        }

        public async Task AddOrUpdatePermissionsToProfile(List<ProfilePermission> profilePermissions)
        {
            _context.Set<ProfilePermission>().UpdateRange(profilePermissions);
            await SaveChangesAsync();
        }

        public void DeleteProfilePermissionsByProfileId(long profileId)
        {
            var profilePermissions = _context.Set<ProfilePermission>().AsNoTracking().AsQueryable();
            var permissionsToDeleted = profilePermissions.Where(x => x.ProfileId == profileId).ToList();

            _context.Set<ProfilePermission>().RemoveRange(permissionsToDeleted);
            SaveChanges();
        }

        public bool IsUsedInSomeProfile(long id)
        {
            var queryProfilePermission = _context.Set<ProfilePermission>().AsQueryable();

            return queryProfilePermission.Where(x => x.PermissionId == id).Any();
        }
    }
}