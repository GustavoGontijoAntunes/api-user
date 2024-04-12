using app.Domain.Models.Filters;
using app.Domain.Services;
using app.WebApi.Dtos.Requests;
using app.WebApi.Dtos.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile = app.Domain.Models.Profile;

namespace app.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService,
            IMapper mapper) : base(mapper)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Post a profile in app.
        /// </summary>
        /// <param name="profilePost">Profile post</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "POST_PROFILE")]
        public IActionResult Post([FromBody] ProfilePost profilePost)
        {
            var profile = _mapper.Map<Profile>(profilePost);
            _profileService.Add(profile, User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Edit a profile in app.
        /// </summary>
        /// <param name="profilePost">Profile post</param>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "PUT_PROFILE")]
        public IActionResult Edit([FromBody] ProfilePost profilePost)
        {
            var profile = _mapper.Map<Profile>(profilePost);
            _profileService.Update(profile, User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Get a profile by id in app.
        /// </summary>
        /// <param name="id">Profile Id</param>
        /// <returns>Profile object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PROFILE")]
        public ActionResult<ProfileResult> GetById(long id)
        {
            var profile = _profileService.GetById(id);
            var result = _mapper.Map<ProfileResult>(profile);

            return Ok(result);
        }

        /// <summary>
        /// Get all profiles in app.
        /// </summary>
        /// <returns>List of profiles</returns>
        [HttpGet("all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "GET_PROFILE")]
        public ActionResult<CollectionResult<ProfileResult>> GetAll([FromQuery] ProfileGet profileGet)
        {
            var search = _mapper.Map<ProfileSearch>(profileGet);
            var profiles = _profileService.GetAll(search);
            var result = _mapper.Map<CollectionResult<ProfileResult>>(profiles);

            return Ok(result);
        }

        /// <summary>
        /// Delete a profile in app.
        /// </summary>
        /// <param name="id">Profile id</param>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "DELETE_PROFILE")]
        public IActionResult DeleteById([FromQuery] long id)
        {
            _profileService.DeleteById(id, User.Identity.Name);

            return Ok();
        }
    }
}