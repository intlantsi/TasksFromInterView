using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Xunit;
using CSBI.Services;
using CSBI.Interfaces;
using CSBI.Models;
using CSBI.Middlewares;


namespace CSBI.Tests
{
    public class SecureAdminMiddlewareTests
    {
        public SecureAdminMiddlewareTests()
        {

        }

        [Fact]
        public async Task Middleware_ReplaceUserId_Ok()
        {
            // Arrange:
            HttpContext ctx = new DefaultHttpContext();
            ctx.Request.Headers.Add("secure-admin", "e98d2243-8fd5-40f6-990f-48ca312b8aa5");
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
            SecureAdminMiddleware mdlWare = new SecureAdminMiddleware(next);

            //Act
            await mdlWare.InvokeAsync(ctx);

            //Assert
            Assert.True(ctx.User.Identity.IsAuthenticated);
        }

        [Fact]
        public async Task Middleware_ReplaceUserId_Cancel()
        {
            // Arrange:
            HttpContext ctx = new DefaultHttpContext();
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
            SecureAdminMiddleware mdlWare = new SecureAdminMiddleware(next);

            //Act
            await mdlWare.InvokeAsync(ctx);

            //Assert
            Assert.False(ctx.User.Identity.IsAuthenticated);
        }
    }
}
