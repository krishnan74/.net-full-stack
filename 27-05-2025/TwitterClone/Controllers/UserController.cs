using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace TwitterClone.Controllers{
    [ApiController]
    [Route("/api/[controller]")]

    public class UserController: ControllerBase{

        private readonly IUserService _userService;
        public UserController( IUserService userService )
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        public ActionResult<User> PutUser([FromBody] User user)
        {
            if (user == null || user.Id <= 0)
            {
                return BadRequest("Invalid user data");
            }
            var updatedUser = _userService.UpdateUser(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}