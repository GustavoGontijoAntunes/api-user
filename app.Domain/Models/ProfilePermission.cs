using app.Domain.Models.Base;

namespace app.Domain.Models
{
    public class ProfilePermission : BaseModel
    {
        public ProfilePermission() { }

        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
        public long PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

    public class ProfilePermissionComparer : IEqualityComparer<ProfilePermission>
    {
        public bool Equals(ProfilePermission x, ProfilePermission y)
        {
            // Checks if descriptions and values are the same
            return x.ProfileId == y.ProfileId && x.PermissionId == y.PermissionId;
        }

        public int GetHashCode(ProfilePermission profilePermission)
        {
            // Creates a unique code hash based on object properties
            return HashCode.Combine(profilePermission.ProfileId, profilePermission.PermissionId);
        }
    }
}
