using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services.Auth
{
   public interface IAuthService
    {
         
        string GenerateToken(DateTime date, string user, TimeSpan validDate);
    }
}
