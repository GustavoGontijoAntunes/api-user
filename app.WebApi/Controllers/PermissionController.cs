using app.Domain.Models;
using app.Domain.Services;
using app.WebApi.Dtos.Requests;
using app.WebApi.Dtos.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService,
            IMapper mapper) : base(mapper)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Post a permission in app.
        /// </summary>
        /// <param name="permissionPost">Permission post</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "POST_PERMISSION")]
        public async Task<IActionResult> Post([FromBody] PermissionPost permissionPost)
        {
            var permission = _mapper.Map<Permission>(permissionPost);
            await _permissionService.Add(permission);

            return Ok();
        }

        /// <summary>
        /// Edit a permission in app.
        /// </summary>
        /// <param name="permissionPost">Permission post</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "PUT_PERMISSION")]
        public async Task<IActionResult> Edit([FromBody] PermissionPost permissionPost)
        {
            var permission = _mapper.Map<Permission>(permissionPost);
            await _permissionService.Update(permission);

            return Ok();
        }

        /// <summary>
        /// Get a permission by id in app.
        /// </summary>
        /// <param name="id">Permission Id</param>
        /// <returns>Permission object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PERMISSION")]
        public ActionResult<PermissionResult> GetById(long id)
        {
            var permission = _permissionService.GetById(id);
            var result = _mapper.Map<PermissionResult>(permission);

            return Ok(result);
        }

        /// <summary>
        /// Get all permissions in app.
        /// </summary>
        /// <returns>List of permissions</returns>
        [HttpGet("all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PERMISSION")]
        public ActionResult<IEnumerable<PermissionResult>> GetAll()
        {
            var permissions = _permissionService.GetAll();
            var result = _mapper.Map<IEnumerable<PermissionResult>>(permissions);

            return Ok(result);
        }

        /// <summary>
        /// Delete a permission in app.
        /// </summary>
        /// <param name="id">Permission id</param>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "DELETE_PERMISSION")]
        public IActionResult DeleteById([FromQuery] long id)
        {
            _permissionService.DeleteById(id);

            return Ok();
        }
    }
}