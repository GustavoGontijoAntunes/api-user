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
        Task Add(Permission permission);
        Task Update(Permission permission);
        Task AddOrUpdateRange(List<Permission> permissions);
        void DeleteById(long id);
        List<Permission> GetPermissionsByProfileId(long profileId);
        Excel GetExcel();
        Excel GetExcelModel();
        Task RegisterByExcel(IFormFile file);
    }
}