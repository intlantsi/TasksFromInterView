using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CSBI.Interfaces;
using CSBI.Models;


namespace CSBI.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        IAppContext _db;
        IUserService _userService;

        public UsersController(IAppContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public ActionResult RegisterUser([FromBody] Credentials newUser)
        {
            if (!_userService.CheckUserCredential(newUser))
                return BadRequest("User credential is wrong!");

            var existUser = _db.Users.FirstOrDefault(x => x.Login == newUser.Login);

            if (existUser != null)
                return BadRequest("User already exist!");

            var userToAdd = new User() { Login = newUser.Login, Password = newUser.Password };

            _db.Users.Add(userToAdd);
            _db.SaveChanges();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authorization")]
        public ActionResult<string> AuthorizeUser([FromBody] Credentials userToAuth)
        {
            if (!_userService.CheckUserCredential(userToAuth))
                return BadRequest("User credential is wrong!");

            var existUser = _db.Users.FirstOrDefault(x => x.Login == userToAuth.Login);

            if (existUser == null)
                return NotFound("User doesn't exist!");

            if(existUser.Password!= userToAuth.Password)
                return BadRequest("Password incorrect!");

            string token=_userService.GenerateJwtToken(existUser.Id);

            existUser.SuccessAuthorizes.Add(new SuccessAuthorize()
            {
                AuthorizeTime = DateTime.Now,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            });

            _db.SaveChanges();
            return token;
        }

        [HttpGet]
        [Route("authorization/list")]
        public ActionResult<List<SuccessAuthorize>> GetAuthorList()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(claimsIdentity.FindFirst("Id").Value);
            var user = _db.Users.Include(x => x.SuccessAuthorizes).FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return NotFound("User doesn't exist!");

            return user.SuccessAuthorizes;
        }

        [HttpGet]
        [Route("{id}/authorization/list")]
        public ActionResult<List<SuccessAuthorize>> GetAuthorListById(Guid id)
        {
            var authorizes = _db.SuccessAuthorizes.Where(x => x.User.Id == id).ToList();
            
            if (authorizes.Count==0)
                return NotFound("Authorizes for user not found!");

            return authorizes;
        }
    }
}
