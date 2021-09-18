using Microsoft.Extensions.Logging;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services
{
    public class ClientService : IClientService
    {
        private readonly NoeliaStorytellerAPIContext _context;
        //private readonly ILogger<ClientService> logger;

        //public  ClientService(NoeliaStorytellerAPIContext context, ILogger<ClientService> logger)
        //{
        //    _context = context;
        //    this.logger = logger;
        //}

        public async Task<Client> getClientAsync(string email)
        {
            Client client =  await _context.Client.FindAsync(email);
            return client;
        }

       

        

    }
}
