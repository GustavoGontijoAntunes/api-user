using app.Domain.Models.Authentication;
using app.Domain.Services;
using app.WebApi.Dtos.Requests;
using app.WebApi.Dtos.Results;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService,
            IMapper mapper) : base(mapper)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login in app.
        /// </summary>
        /// <param name="userLogin">User login</param>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserAuthenticatedResponse>> Login([FromBody] UserLogin userLogin)
        {
            userLogin.ThrowIfNotValid();
            var user = _mapper.Map<User>(userLogin);
            var result = await _userService.Login(user);

            if (result == null || !result.Authenticated)
            {
                return Unauthorized(result?.Message);
            }

            var response = result;

            return Ok(response);
        }
    }
}