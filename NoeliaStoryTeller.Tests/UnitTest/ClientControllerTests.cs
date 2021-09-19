//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoeliaStorytellerAPI.Controllers;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeliaStoryTeller.Tests.UnitTest
{
    [TestClass]
   public class ClientControllerTests : BaseTest
    {
        private NoeliaStorytellerAPIContext _context;
        private ClientsController _clientController;
        private  ILogger<ClientsController> logger;
        private IAuthService _authService;
        [TestMethod]
        public async Task ClientCreate()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<NoeliaStorytellerAPIContext>()
                .UseInMemoryDatabase("NoeliaStorytellerAPIContext").Options;

            _context = new NoeliaStorytellerAPIContext(options);
            _clientController = new ClientsController(_context, logger, _authService);
            Client client = new ();
            client.UserName = "Test1";
            client.Email = "test@test.com";

          var clientCreated =  await _clientController.PostClient(client);
            var cli = await _clientController.GetClient(client.Email);
            var result = (clientCreated.Result as ObjectResult)?.StatusCode;
            //var status = result as StatusCodeResult;
            Assert.AreEqual(201, result);
        }

        [TestMethod]
        public async Task ClientTokenCreate_NoClientRegister()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<NoeliaStorytellerAPIContext>()
                .UseInMemoryDatabase(dbName).Options;
            _context = new NoeliaStorytellerAPIContext(options);            
            _clientController = new ClientsController(_context, logger, _authService);
            Client client = new Client();
            client.UserName = "Test1";
            client.Email = "test@test.com";

            var TokenCreated = await _clientController.Token(client);
            //var result = await TokenCreated
            var result = (TokenCreated as ObjectResult)?.StatusCode;
            var value = (TokenCreated as ObjectResult)?.Value;
            Assert.AreEqual(401, result );
            //Assert.IsNotNull(TokenCreated);
        }

        [TestMethod]
        public async Task ClientTokenCreate_GetToken()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<NoeliaStorytellerAPIContext>()
                .UseInMemoryDatabase(dbName).Options;            
            _context = new NoeliaStorytellerAPIContext(options);
            _clientController = new ClientsController(_context, logger, _authService);
            Client client = new Client();
            client.UserName = "Test1";
            client.Email = "test@test.com";

            var clientCreated = await _clientController.PostClient(client);
            var cli = await _clientController.GetClient(client.Email);
            var TokenCreated = await _clientController.Token(client);
            var result = (TokenCreated as ObjectResult)?.StatusCode;
            var value = (TokenCreated as ObjectResult)?.Value;
            Assert.AreEqual(201, result);

        }
    }
}
