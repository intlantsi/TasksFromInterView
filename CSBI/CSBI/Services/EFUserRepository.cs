using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSBI.Interfaces;
using CSBI.Models;

namespace CSBI.Services
{
    public class EFUserRepository:IUserRepository
    {
        AppContext context;

        public EFUserRepository(AppContext efContext)
        {
            context = efContext;
        }

        public void Create(User user)
        {
            context.Users.Add(user);
        }

        public User GetByName(string userName)
        {
            return context.Users.FirstOrDefault(x => x.Login == userName);
        }

        public User GetByIdWithAuthor(Guid userId)
        {
            return context.Users.Include(x => x.SuccessAuthorizes).FirstOrDefault(x => x.Id == userId);
        }

        public void Update(User user)
        {
            context.Users.Update(user);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
