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
    public class GetAuthorListByIdTests
    {
        IConfiguration config;
        IUserService userService;
        IUserRepository userRepo;

        public GetAuthorListByIdTests()
        {
            var myConfiguration = new Dictionary<string, string> { { "Token:Key", "SuperSecretTestKey" } };
            config = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            userService = new UserService(config);
            userRepo = new MockUserRepo();
        }

        [Fact]
        public void GetAuthorListById_UserNotExist_NotFound()
        {
            //Arrange
            UsersController cntrl = new UsersController(userRepo, userService);

            //Act
            var response = cntrl.GetAuthorListById(Guid.Empty);

            //Assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public void GetAuthorListById_Succesfull()
        {
            //Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Login = "Login",
                Password = "Password",
            };
            user.SuccessAuthorizes.Add(new SuccessAuthorize { Id = 1, IP = "127.0.0.1", AuthorizeTime = DateTime.Now });
            userRepo.Create(user);
            userRepo.SaveChanges();
            UsersController cntrl = new UsersController(userRepo, userService);


            //Act
            var response = cntrl.GetAuthorListById(user.Id);

            //Assert
            Assert.NotEmpty(response.Value);
        }
    }
}
