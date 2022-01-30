using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Xunit;
using CSBI.Services;
using CSBI.Interfaces;
using CSBI.Models;
using CSBI.Controllers;

namespace CSBI.Tests.Controllers.UserController
{
    public class AuthorizeUserTests
    {
        IConfiguration config;
        IUserService userService;
        IAppContext context;

        public AuthorizeUserTests()
        {
            var myConfiguration = new Dictionary<string, string> { { "Token:Key", "SuperSecretTestKey" } };
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            userService = new UserService(config);

            var options = new DbContextOptionsBuilder<AppContext>()
                    .UseInMemoryDatabase(databaseName: "CSBIDB")
                    .Options;
            context = new AppContext(options);
            context.Users.Add(new User() { Id = Guid.NewGuid(), Login = "Login", Password = "Password" });
            context.SaveChanges();
        }

        [Fact]
        public void AuthorizeUser_Succesfull_Ok()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);
            cntrl.ControllerContext.HttpContext = new DefaultHttpContext();
            cntrl.ControllerContext.HttpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            //Act
            var response = cntrl.AuthorizeUser(new Credentials { Login = "Login", Password = "Password" });

            //Assert
            Assert.NotNull(response.Value);
        }

        [Fact]
        public void AuthorizeUser_UserIsNull_BadRequest()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);

            //Act
            var response = cntrl.AuthorizeUser(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public void AuthorizeUser_LoginOrPasswordIsNull_BadRequest()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);

            //Act
            var response = cntrl.AuthorizeUser(new Credentials { Login = null, Password = null });

            //Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public void AuthorizeUser_UserNotExist_NotFound()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);

            //Act
            var response = cntrl.AuthorizeUser(new Credentials { Login = "Login12", Password = "Password" });

            //Assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public void AuthorizeUser_PswdIncorrect_BadRequest()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);

            //Act
            var response = cntrl.AuthorizeUser(new Credentials { Login = "Login", Password = "111111111" });

            //Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }
    }
}
