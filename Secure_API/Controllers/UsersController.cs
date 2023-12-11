using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Secure_API.Models;
using Secure_API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Secure_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Admin")]
    public class UsersController : ControllerBase
    {

        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//userA prøver på userB
        public ActionResult<User> Get(Guid id)
        {
            Guid userIdGuid = GetIdFromToken();
            if (userIdGuid == id)
            {
                User? user = _usersRepository.GetById(userIdGuid);
                if (user == null) return NotFound();
                return Ok(user);

            }
            return Forbid();
        }

        // PUT api/<UsersController>/5
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//token er udløbet
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//userA prøver på userB
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [HttpPut("{id}")]
        public ActionResult<User> Put(Guid id, [FromBody] User newData)
        {
            if (GetIdFromToken() == id)
            {
                User userToUpdate = _usersRepository.Update(id, newData);
                if (userToUpdate == null) return NotFound();
                return Ok(userToUpdate);
            }
            return Forbid();
        }
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//token er udløbet
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//userA prøver på userB
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [HttpPut("{id}/changePassword")]
        public ActionResult<User> PutPassword(Guid id, [FromBody] UserCredentials newData)
        {

            try
            {
                if (GetIdFromToken() == id)
                {
                    User user = _usersRepository.ChangePassword(id, newData);
                    if (user == null) return NotFound();
                    return Ok(user);
                }
                return Forbid();
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private Guid GetIdFromToken()
        {
            string userId = this.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            Guid userIdGuid = Guid.Parse(userId);
            return userIdGuid;
        }
        

    }
}
