using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;
using CSBI.Services;
using CSBI.Interfaces;
using CSBI.Models;
using CSBI.Controllers;
using CSBI.Tests.Mocks;

namespace CSBI.Tests.Controllers.UserController
{
    public class RegisterUserTests
    {
        IConfiguration config;
        IUserService userService;
        IUserRepository userRepo;

        public RegisterUserTests()
        {
            var myConfiguration = new Dictionary<string, string> { { "Token:Key", "SuperSecretTestKey" } };
            config = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            userService = new UserService(config);
            userRepo = new MockUserRepo();
        }

        [Fact]
        public void RegisterUser_Succesfull()
        {
            //Arrange
            UsersController _cntrl = new UsersController(userRepo, userService);

            //Act
			var response=_cntrl.RegisterUser(new Credentials { Login = "LoginNew", Password = "PasswordNew" });

            //Assert
            Assert.IsType<OkResult>(response);
        }
		
		[Fact]
        public void RegisterUser_UserIsNull()
        {
            //Arrange
            UsersController _cntrl = new UsersController(userRepo, userService);

            //Act
            var response=_cntrl.RegisterUser(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }
		
		[Fact]
        public void RegisterUser_LoginOrPasswordIsNull_BadRequest()
        {
            //Arrange
            UsersController _cntrl = new UsersController(userRepo, userService);

            //Act
            var response=_cntrl.RegisterUser(new Credentials { Login = null, Password = null });

            //Assert
			Assert.IsType<BadRequestObjectResult>(response);
        }
		
		[Fact]
        public void RegisterUser_UserAlreadyExist()
        {
            //Arrange
            userRepo.Create(new User() { Id = Guid.NewGuid(), Login = "Login", Password = "Password" });
            UsersController _cntrl = new UsersController(userRepo, userService);

            //Act
			var response=_cntrl.RegisterUser(new Credentials { Login = "Login", Password = "Password" });

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
