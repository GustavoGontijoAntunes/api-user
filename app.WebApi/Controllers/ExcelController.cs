using app.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ExcelController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public ExcelController(
            IPermissionService permissionService,
            IMapper mapper) : base(mapper)
        {
            _permissionService = permissionService;
        }

        #region Download
        /// <summary>
        /// Performs permissions search and generate a excel file
        /// </summary>
        /// <returns>Permission excel file</returns>
        [HttpGet("Permission")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PERMISSION")]
        public IActionResult DownloadPermissions()
        {
            var excel = _permissionService.GetExcel();
            return FileExcel(excel);
        }

        /// <summary>
        /// Performs permission model excel
        /// </summary>
        /// <returns>Permission excel model file</returns>
        [HttpGet("PermissionModel")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PERMISSION")]
        public IActionResult DownloadPermissionModel()
        {
            var excel = _permissionService.GetExcelModel();
            return FileExcel(excel);
        }
        #endregion

        #region Upload
        /// <summary>
        /// Import permission data
        /// </summary>
        /// <param name="file">Excel File</param>
        [HttpPost("Permission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "POST_PERMISSION")]
        public async Task<IActionResult> UploadPermission(IFormFile file)
        {
            await _permissionService.RegisterByExcel(file);

            return Ok();
        }
        #endregion
    }
}