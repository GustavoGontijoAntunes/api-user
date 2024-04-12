using app.Domain.Models;
using app.Domain.Models.Excel;
using Microsoft.AspNetCore.Http;

namespace app.Domain.Adapter
{
    public interface IExcelAdapter
    {
        /// <summary>
        /// Read excel file exactly like domain model
        /// </summary>
        /// <param name="file">The excel file with data</param>
        /// <returns>A collection of perissions</returns>
        /// <exception cref="Exceptions.ReadExcelException">Error when trying to read excel data</exception>
        public List<Permission> ReadPermission(IFormFile file);

        /// <summary>
        /// Generate a excel with permissions data
        /// </summary>
        /// <param name="permissions">The permissions list</param>
        /// <returns>Excel model of permissions </returns>
        public Excel GetPermission(IEnumerable<Permission> permissions);

        /// <summary>
        /// Generate a excel model to products
        /// </summary>
        /// <returns>Product excel model</returns>
        public Excel GetPermissionModel();
    }
}