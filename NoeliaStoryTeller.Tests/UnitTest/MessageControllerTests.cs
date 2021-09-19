using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoeliaStorytellerAPI.Controllers;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.DTO;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Auth;
using NoeliaStorytellerAPI.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeliaStoryTeller.Tests.UnitTest
{
    [TestClass]
    public class MessageControllerTests
    {
        private NoeliaStorytellerAPIContext _context;
        private ILogger<MessageItemsController> logger;
        private MessageItemsController _messageController;
        private IMessageService _messageService;
        private ClientsController _clientController;
        private ILogger<ClientsController> loggerC;
        private IAuthService _authService;
        [TestMethod]

        public async Task MessageCreate_OK()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message created by test";
            message.client = client;
            var clientCreated = CreateClient(client);
            _messageController = new MessageItemsController(_context, _messageService, logger);
            var created = await _messageController.CreateMessage(message);
            var result = (created.Result as ObjectResult)?.StatusCode;

            Assert.AreEqual(201, result);
        }
        [TestMethod]
        public async Task MessageCreate_NotOk()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);

            MessageOperationsDTO message = new();
            //var clientCreated = CreateClient();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message not created by test";
            message.client = client;
            _messageController = new MessageItemsController(_context, _messageService, logger);
            var created = await _messageController.CreateMessage(message);
            var result = (created.Result as ObjectResult)?.StatusCode;

            Assert.AreEqual(401, result);
        }
        [TestMethod]

        public async Task GetMessageByEmail_OK()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);
            _messageController = new MessageItemsController(_context, _messageService, logger);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message from test1";
            message.client = client;

            var clientCreated = CreateClient(client);

            MessageOperationsDTO message2 = new();            
            message2.Message = "Message 2";
            message2.client = client;

            var createMd = await _messageController.CreateMessage(message);
            var createdM2 = await _messageController.CreateMessage(message2);


            var messages = await _messageController.GetMessageByEmail(client.Email);
            var result = (messages as OkObjectResult)?.StatusCode;

            Assert.AreEqual(200, result);
        }

        [TestMethod]

        public async Task GetAllMessage_OK()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);
            _messageController = new MessageItemsController(_context, _messageService, logger);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message from test1";
            message.client = client;

            var clientCreated = CreateClient(client);

            MessageOperationsDTO message2 = new();
            Client client2 = new();
            client2.UserName = "Test1";
            client2.Email = "Noelia@test.com";
            message2.Message = "Message from Noelia";
            var clientCreated2 = CreateClient(client2);
            message2.client = client2;
            var createMd = await _messageController.CreateMessage(message);
            var createdM2 = await _messageController.CreateMessage(message2);


            var allMessages = await _messageController.GetClientsAllMessages(client);
            var result = (allMessages as OkObjectResult)?.StatusCode;

            Assert.AreEqual(200, result);
        }
        [TestMethod]

        public async Task GetAllMessage_Unauthorized()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);
            _messageController = new MessageItemsController(_context, _messageService, logger);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message from test1";
            message.client = client;

            var clientCreated = CreateClient(client);

            MessageOperationsDTO message2 = new();
            Client client2 = new();
            client2.UserName = "Test1";
            client2.Email = "Noelia@test.com";
            message2.Message = "Message from Noelia";
            var clientCreated2 = CreateClient(client2);
            message2.client = client2;
            var createMd = await _messageController.CreateMessage(message);
            var createdM2 = await _messageController.CreateMessage(message2);

            Client client3 = new();
            client3.UserName = "Test3";
            client3.Email = "nouser@test.com";
            var allMessages = await _messageController.GetClientsAllMessages(client3);
            var result = (allMessages as ObjectResult)?.StatusCode;

            Assert.AreEqual(401, result);
        }
        [TestMethod]
        public async Task UpdateMessage_OK()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message created by test to be updated";
            message.client = client;
            var clientCreated = CreateClient(client);
            _messageController = new MessageItemsController(_context, _messageService, logger);
            var created = await _messageController.CreateMessage(message);
            message.Id = 1;
            message.Message = "Message updated";
            var updated = await _messageController.UpdateMessage(1, message);
            var result = (updated as ObjectResult)?.StatusCode;

            Assert.AreEqual(201, result);
        }

        [TestMethod]
        public async Task UpdateMessage_Unauthorized()
        {
            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message created by test to be updated";
            message.client = client;
            var clientCreated = CreateClient(client);
            _messageController = new MessageItemsController(_context, _messageService, logger);
            var created = await _messageController.CreateMessage(message);
            message.Id = 1;
            message.Message = "Message updated";
            message.client.Email = "nouser@nouser.com";
            var updated = await _messageController.UpdateMessage(1, message);
            var result = (updated as ObjectResult)?.StatusCode;

            Assert.AreEqual(401, result);
        }

        [TestMethod]
        public async Task DeleteMessage_OK()
        {
            var dbName = Guid.NewGuid().ToString();

            var options = setContext();
            _context = new NoeliaStorytellerAPIContext(options);

            MessageOperationsDTO message = new();
            Client client = new();
            client.UserName = "Test1";
            client.Email = "test@test.com";
            message.Message = "Message created by test";
            message.client = client;
            var clientCreated = CreateClient(client);
            _messageController = new MessageItemsController(_context, _messageService, logger);
            var created = await _messageController.CreateMessage(message);
            message.Id = 1;
            var deleted = _messageController.DeleteMessage(1, client);
            var result = (deleted.Result as OkResult)?.StatusCode;

            Assert.AreEqual(200, result);
        }
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            _clientController = new ClientsController(_context, loggerC, _authService);
      

            var clientCreated = await _clientController.PostClient(client);
            var cli = await _clientController.GetClient(client.Email);
            return cli;
        }
        public DbContextOptions<NoeliaStorytellerAPIContext> setContext()
        {
            return  new DbContextOptionsBuilder<NoeliaStorytellerAPIContext>()
               .UseInMemoryDatabase("NoeliaStorytellerAPIContext").Options;
        }
    }
}
