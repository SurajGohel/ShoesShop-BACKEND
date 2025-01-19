using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Data;
using ShoesShop.Models;

namespace ShoesShop.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You have accessed the User controller.");
        }

        //private readonly UserRepository _userRepository;

        //public UserController(UserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        //[HttpGet]
        //public IActionResult GetAllUserss()
        //{
        //    var users = _userRepository.SelectAll();
        //    return Ok(users);
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetUserById(int id)
        //{
        //    var user = _userRepository.SelectByPK(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteUser(int id)
        //{
        //    var isDel = _userRepository.Delete(id);
        //    if (!isDel)
        //        return NotFound();
        //    return Ok(new { message = "User deleted successfully" });
        //}

        //[HttpPost]
        //public IActionResult InsertProduct([FromBody] UserModel user)
        //{
        //    if (user == null)
        //    {
        //        //Console.WriteLine(city.CityID);
        //        return BadRequest();
        //    }
        //    bool isInserted = _userRepository.Insert(user);

        //    if (isInserted)
        //    {
        //        return Ok(new { Message = "User Inserted Succesfully" });
        //    }

        //    return StatusCode(500, "An Error Occured While Inserting Product");
        //}

        //[HttpPut("{id}")]
        //public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        //{
        //    if (user == null || id != user.UserId)
        //        return BadRequest();

        //    var isUpdate = _userRepository.Update(user);
        //    if (!isUpdate)
        //        return NotFound();
        //    return Ok(new { message = "User deleted successfully" });
        //}


        //[HttpGet("Login/{e}/{p}/{r}")]
        //public IActionResult Login(string e,string p,string r)
        //{   
        //    var user = _userRepository.Login(e,p,r);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}
    }
}
