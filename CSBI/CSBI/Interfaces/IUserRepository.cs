using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSBI.Interfaces;
using CSBI.Models;

namespace CSBI.Interfaces
{
    public interface IUserRepository
    {
        User GetByName (string userName);
        User GetByIdWithAuthor(Guid id);
        void Create(User user);
        void Update(User user);
        void SaveChanges();
    }
}
