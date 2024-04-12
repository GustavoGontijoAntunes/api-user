using app.Domain.Models.Filters;
using app.Domain.Services;
using app.WebApi.Dtos.Requests;
using app.WebApi.Dtos.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    public class AuditController : BaseController
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService, IMapper mapper) : base(mapper)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Get all audits data in app.
        /// </summary>
        /// <returns>List of audits data</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "VIS_LOG_AUDIT")]
        public ActionResult<CollectionResult<AuditResult>> Get([FromQuery] AuditGet auditGet)
        {
            var search = _mapper.Map<AuditSearch>(auditGet);
            var auditList = _mapper.Map<CollectionResult<AuditResult>>(_auditService.Get(search));

            return Ok(auditList);
        }
    }
}