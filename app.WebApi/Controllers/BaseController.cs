using app.Domain.Models.Authentication;
using app.Domain.Models.Excel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

namespace app.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        protected BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IActionResult FileExcel(Excel result)
        {
            return File(result.Content,
                        result.MymeType,
                        result.Name);
        }

        private string userToken => HttpContext.Request.Headers.ContainsKey("Authorization")
            ? Convert.ToString(Request.Headers["Authorization"]).Trim().Replace("Bearer ", string.Empty)
            : string.Empty;
        private string NameCurrentUser => User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        private IEnumerable PermissionsUser => User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(f => f.Value);

        public User BaseUser
        {
            get
            {
                var currentUser = new User()
                {
                    Name = NameCurrentUser,
                    Login = User.Identity.Name,
                };

                currentUser.Permissions = PermissionsUser;
                currentUser.Token = userToken;

                return currentUser;
            }
        }
    }
}