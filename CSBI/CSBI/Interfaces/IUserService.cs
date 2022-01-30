using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CSBI.Models;

namespace CSBI.Interfaces
{
    public interface IUserService
    {
        string GenerateJwtToken(Guid userId);
        bool CheckUserCredential(Credentials cred);
    }
}
