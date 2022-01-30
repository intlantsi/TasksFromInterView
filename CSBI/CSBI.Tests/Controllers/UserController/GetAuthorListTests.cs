using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
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
    public class GetAuthorListTests
    {
        IConfiguration config;
        IUserService userService;
        IAppContext context;

        public GetAuthorListTests()
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
        }

        [Fact]
        public void GetAuthorList_UserNotExist_NotFound()
        {
            //Arrange
            UsersController cntrl = new UsersController(context, userService);
            cntrl.ControllerContext.HttpContext = new DefaultHttpContext();
            cntrl.ControllerContext.HttpContext.User 
                = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", Guid.Empty.ToString()) }, "AuthenticationTypes.Federation"));
            
            //Act
            var response = cntrl.GetAuthorList();

            //Assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public void GetAuthorList_Succesfull()
        {
            //Arrange
            User user = new User()
            {
                Id=Guid.NewGuid(),
                Login = "Login",
                Password = "Password",
            };
            user.SuccessAuthorizes.Add(new SuccessAuthorize { Id = 2, IP = "127.0.0.1", AuthorizeTime = DateTime.Now });
            context.Users.Add(user);
            context.SaveChanges();

            UsersController cntrl = new UsersController(context, userService);
            cntrl.ControllerContext.HttpContext = new DefaultHttpContext();
            cntrl.ControllerContext.HttpContext.User
                = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }, "AuthenticationTypes.Federation"));

            //Act
            var response = cntrl.GetAuthorList();

            //Assert
            Assert.NotEmpty(response.Value);
        }
    }
}
