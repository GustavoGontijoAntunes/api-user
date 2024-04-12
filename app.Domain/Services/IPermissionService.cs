using app.Domain.Models;
using app.Domain.Models.Excel;
using Microsoft.AspNetCore.Http;

namespace app.Domain.Services
{
    public interface IPermissionService
    {
        IEnumerable<Permission> GetAll();
        Permission GetByName(string name);
        Permission GetById(long id);
        void Add(Permission permission, string userName);
        void Update(Permission permission, string userName);
        void AddOrUpdateRange(List<Permission> permissions, string userName);
        void DeleteById(long id, string userName);
        List<Permission> GetPermissionsByProfileId(long profileId);
        Excel GetExcel();
        Excel GetExcelModel();
        void RegisterByExcel(IFormFile file, string userName);
    }
}