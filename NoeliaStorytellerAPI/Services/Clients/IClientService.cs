using NoeliaStorytellerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services.Clients
{
   public interface IClientService 
    {
        Task<Client> getClientAsync(string email);
     

    }
}
