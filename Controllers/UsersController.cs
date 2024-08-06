using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITKANSys_api.Services;
using ITKANSys_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("GetConcernedUsers/{roleId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetConcernedUsersByRole(int roleId)
        {
            var users = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(users);
        }
        [HttpGet("getUsersByRole")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByRole([FromQuery] List<int> roleIds)
        {
            var users = await _userService.GetUserByRolesAsync(roleIds);
            if (users == null || !users.Any())
            {
                return NotFound("Aucun utilisateur trouvé pour les rôles spécifiés.");
            }
            return Ok(users);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound("Utilisateur non trouvé.");
            }
            return NoContent();
        }



    }
}

