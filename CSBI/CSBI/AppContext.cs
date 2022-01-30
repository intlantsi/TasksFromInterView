using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSBI.Models;
using CSBI.Interfaces;

namespace CSBI
{
    public class AppContext:DbContext, IAppContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options) 
        {
            this.Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SuccessAuthorize> SuccessAuthorizes { get; set; }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
