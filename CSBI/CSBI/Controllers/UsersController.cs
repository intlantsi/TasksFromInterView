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
        IUserRepository userRepo;
        IUserService userService;

        public UsersController(IUserRepository userRepo, IUserService userService)
        {
            this.userRepo = userRepo;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public ActionResult RegisterUser([FromBody] Credentials newUser)
        {
            if (!userService.CheckUserCredential(newUser))
                return BadRequest("User credential is wrong!");

            var existUser = userRepo.GetByName(newUser.Login);

            if (existUser != null)
                return BadRequest("User already exist!");

            var userToAdd = new User() { Login = newUser.Login, Password = newUser.Password };

            userRepo.Create(userToAdd);
            userRepo.SaveChanges();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authorization")]
        public ActionResult<string> AuthorizeUser([FromBody] Credentials userToAuth)
        {
            if (!userService.CheckUserCredential(userToAuth))
                return BadRequest("User credential is wrong!");

            var existUser = userRepo.GetByName(userToAuth.Login);

            if (existUser == null)
                return NotFound("User doesn't exist!");

            if(existUser.Password!= userToAuth.Password)
                return BadRequest("Password incorrect!");

            string token= userService.GenerateJwtToken(existUser.Id);

            existUser.SuccessAuthorizes.Add(new SuccessAuthorize()
            {
                AuthorizeTime = DateTime.Now,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            });

            userRepo.Update(existUser);
            userRepo.SaveChanges();
            return token;
        }

        [HttpGet]
        [Route("authorization/list")]
        public ActionResult<List<SuccessAuthorize>> GetAuthorList()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(claimsIdentity.FindFirst("Id").Value);
            var user = userRepo.GetByIdWithAuthor(userId);

            if (user == null)
                return NotFound("User doesn't exist!");

            if (user.SuccessAuthorizes.Count == 0)
                return NotFound("Authorizes for user not found!");

            return user.SuccessAuthorizes;
        }

        [HttpGet]
        [Route("{id}/authorization/list")]
        public ActionResult<List<SuccessAuthorize>> GetAuthorListById(Guid id)
        {
            var user = userRepo.GetByIdWithAuthor(id);

            if (user == null)
                return NotFound("User doesn't exist!");

            if (user.SuccessAuthorizes.Count == 0)
                return NotFound("Authorizes for user not found!");

            return user.SuccessAuthorizes;
        }
    }
}
