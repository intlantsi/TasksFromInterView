using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSBI.Interfaces;
using CSBI.Models;

namespace CSBI.Tests.Mocks
{
    public class MockUserRepo : IUserRepository
    {
        List<User> usersRepo = new List<User>();

        public MockUserRepo()
        {

        }

        public void Create(User user)
        {
            usersRepo.Add(user);
        }

        public User GetByIdWithAuthor(Guid id)
        {
            return usersRepo.FirstOrDefault(x => x.Id == id);
        }

        public User GetByName(string userName)
        {
            return usersRepo.FirstOrDefault(x => x.Login == userName);
        }

        public void SaveChanges()
        {
            return;
        }

        public void Update(User user)
        {
            var existUser=usersRepo.FirstOrDefault(x => x.Login == user.Login);
            existUser.Login = user.Login;
            existUser.Password = existUser.Password;
            existUser.SuccessAuthorizes.AddRange(user.SuccessAuthorizes);
        }
    }
}
