using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.DTO;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services;
using NoeliaStorytellerAPI.Services.Messages;

namespace NoeliaStorytellerAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageItemsController : ControllerBase
    {
        private readonly NoeliaStorytellerAPIContext _context;
        public readonly IMessageService _mservice;
        private readonly ILogger<MessageItemsController> logger;

        public MessageItemsController(NoeliaStorytellerAPIContext context, IMessageService messageService, ILogger<MessageItemsController> logger)
        {
            _context = context;
            _mservice = messageService;
            this.logger = logger;
        }

 
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            logger.LogInformation("Getting all Messages");
           List<MessageItem> messages = await _context.MessageItem.ToListAsync();
            List<MessageItemDTO> mdto = new();
            messages.ForEach(async x =>
            {
                Client c = await _context.Client.FindAsync(x.Email);
                MessageItemDTO m = new MessageItemDTO(x, c);
                mdto.Add(m);
            });

            return Ok(mdto);
        }
       
        [HttpPost("AllMessageByEmail")]
        public async Task<IActionResult> GetClientsAllMessages(Client client)
        {
            //logger.LogInformation("Getting all Messages");
            try
            {
                var cli = _context.Client.Any(e => e.Email == client.Email);
                if (!cli)
                {
                    return StatusCode(401, "Unauthorized");
                }
                List<MessageItem> messages = await _context.MessageItem.ToListAsync();
                if (messages.Count < 0)
                {
                    return NotFound();
                }
                List<MessageItemDTO> mdto = new();
                messages.ForEach(async x =>
                {
                    Client c = await _context.Client.FindAsync(x.Email);
                    MessageItemDTO m = new MessageItemDTO(x, c);
                    mdto.Add(m);
                });

                return Ok(mdto);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetClientsAllMessages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/MessageItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageItem>> GetMessageItem(long id)
        {
            try
            {
                logger.LogInformation("Getting Message by ID", id);
                var messageItem = await _context.MessageItem.FindAsync(id);

                if (messageItem == null)
                {
                    return NotFound();
                }

                return Ok(messageItem);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetMessageItem action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetMessageByEmail(string email)
        {
            try
            {
                //logger.LogInformation("Getting messages by email", email);
                List<MessageItem> messages = await _context.MessageItem.Where(p => p.Email.Contains(email)).ToListAsync();

                if (messages == null)
                {
                    return NotFound();
                }
                List<MessageItemDTO> mdto = new();
                messages.ForEach(async x =>
                {
                    Client c = await _context.Client.FindAsync(x.Email);
                    MessageItemDTO m = new MessageItemDTO(x, c);
                    mdto.Add(m);
                });

                return Ok(mdto);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetMessageByEmail action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }
     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(long id, MessageOperationsDTO messageDTO)
        {
            try
            {
                //logger.LogInformation("Update Message", messageDTO);
                if (id != messageDTO.Id)
                {
                    return BadRequest();
                }
                var messageItem = _context.MessageItem.Find(id);
                if (messageItem.Email != messageDTO.client.Email)
                {
                    return StatusCode(401, "You have not permission to modify this message.");
                }
                messageItem.Message = messageDTO.Message;

                _context.Entry(messageItem).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MessageItemExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        logger.LogError($"Something went wrong inside UpdateMessage action: {ex.Message}");
                        return StatusCode(500, "Internal server error");
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<MessageItem>> CreateMessage(MessageOperationsDTO message)
        {
            try
            {
                //logger.LogInformation("Create Message", message);

                var cli = _context.Client.Any(e => e.Email == message.client.Email);
                if (!cli)
                {
                    return StatusCode(401, "Unauthorized, User is not registered");
                }
                MessageItem messageItem = new();
                messageItem.Email = message.client.Email;
                messageItem.Message = message.Message;
                messageItem.CreationDate = System.DateTime.Now;
                _context.MessageItem.Add(messageItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMessageItem", new { id = messageItem.Id }, messageItem);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/MessageItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id, Client client)
        {
            try
            {
                //logger.LogInformation("Delete Message: ID " + id, client);

                var messageItem = await _context.MessageItem.FindAsync(id);
                if (messageItem == null)
                {
                    return NotFound();
                }
                if(messageItem.Email != client.Email)
                {
                    return StatusCode(401, "You have not permission to detelet this message.");
                }
                _context.MessageItem.Remove(messageItem);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool MessageItemExists(long id)
        {
            return _context.MessageItem.Any(e => e.Id == id);
        }
    }
}
