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

namespace CSBI.Tests
{
    public class UserServiceTest
    {
        IConfiguration config;

        public UserServiceTest()
        {
            var myConfiguration = new Dictionary<string, string> { { "Token:Key", "SuperSecretTestKey" } };
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        [Fact]
        public void CheckUserCredential_CredNull_False()
        {
            //Arrange
            UserService userSrv = new UserService(config);

            //Act
            var result = userSrv.CheckUserCredential(null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckUserCredential_LoginOrPassNull_False()
        {
            //Arrange
            UserService userSrv = new UserService(config);

            //Act
            var result = userSrv.CheckUserCredential(new Credentials { Login = null, Password = null });

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckUserCredential_True()
        {
            //Arrange
            UserService userSrv = new UserService(config);

            //Act
            var result = userSrv.CheckUserCredential(new Credentials { Login = "Login", Password = "Password" });

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GenerateJwtToken_Token()
        {
            //Arrange
            UserService userSrv = new UserService(config);

            //Act
            var result = userSrv.GenerateJwtToken(Guid.Empty);

            //Assert
            Assert.NotNull(result);
        }
    }
}
