using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secure_API.Models;
using Secure_API.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Secure_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: api/<AdminController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//role not allowed
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//token er udløbet
        public ActionResult<IEnumerable<User>> Get()
        {
            IEnumerable<User>? users = _adminRepository.GetAll();
            if (!users.Any())
            {
                return NoContent(); 
            }
            return Ok(users);
        }

        // PUT api/<AdminController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//token er udløbet
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User>? Put(Guid id, [FromBody] string role)
        {
            User? userToUpdate = _adminRepository.AsignRole(id, role);
            if (userToUpdate == null) return NotFound();
            return Ok(userToUpdate);
        }

        // DELETE api/<AdminController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//token er udløbet
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Delete(Guid id)
        {
            User? userToDelete = _adminRepository.Delete(id);
            if (userToDelete == null) return NotFound();
            return Ok(userToDelete);
        }
    }
}
