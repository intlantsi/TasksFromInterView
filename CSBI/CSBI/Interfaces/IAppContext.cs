using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSBI.Models;

namespace CSBI.Interfaces
{
    public interface IAppContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SuccessAuthorize> SuccessAuthorizes { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
