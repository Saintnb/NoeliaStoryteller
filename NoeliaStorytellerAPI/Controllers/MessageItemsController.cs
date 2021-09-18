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
    [Authorize]
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

        // GET: api/MessageItems
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            logger.LogInformation("Getting all Messages");
           List<MessageItem> messages = await _context.MessageItem.ToListAsync();
            List<MessageItemDTO> mdto = new();
           mdto = _mservice.MapMessages(messages);

            return Ok(mdto);
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

        [HttpGet("/byEmail/{email}")]
        public async Task<IActionResult> GetMessageByEmail(string email)
        {
            try
            {
                logger.LogInformation("Getting messages by email", email);
                List<MessageItem> messages = await _context.MessageItem.Where(p => p.Email.Contains(email)).ToListAsync();

                if (messages == null)
                {
                    return NotFound();
                }
                List<MessageItemDTO> mdto = new();
                mdto = _mservice.MapMessages(messages);

                return Ok(mdto);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetMessageByEmail action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        // PUT: api/MessageItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(long id, MessageItem messageItem)
        {
            if (id != messageItem.Id)
            {
                return BadRequest();
            }

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

        // POST: api/MessageItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageItem>> CreateMessage(MessageItem messageItem)
        {
            _context.MessageItem.Add(messageItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageItem", new { id = messageItem.Id }, messageItem);
        }

        // DELETE: api/MessageItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            var messageItem = await _context.MessageItem.FindAsync(id);
            if (messageItem == null)
            {
                return NotFound();
            }

            _context.MessageItem.Remove(messageItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageItemExists(long id)
        {
            return _context.MessageItem.Any(e => e.Id == id);
        }
    }
}
