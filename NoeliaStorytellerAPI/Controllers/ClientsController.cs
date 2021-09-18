using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Auth;
using NoeliaStorytellerAPI.Services.Clients;

namespace NoeliaStorytellerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly NoeliaStorytellerAPIContext _context;
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> logger;
        private readonly IAuthService _authService;
        public ClientsController(NoeliaStorytellerAPIContext context, IClientService clientService, 
            ILogger<ClientsController> logger, IAuthService authService)
        {
            this._context = context;
            this._clientService = clientService;
            this.logger = logger;
            this._authService = authService;
        }


        [HttpGet("{email}")]
        public async Task<ActionResult<Client>> GetClient(string email)
        {
            try
            {
                var client = await _context.Client.FindAsync(email);

                if (client == null)
                {
                    return NotFound();
                }

                return client;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetClient action: {ex.Message}");
                return StatusCode(500, "Internal server error");                
            }
            
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{email}")]
        public async Task<IActionResult> PutClient(string email, Client client)
        {
            if (email != client.Email)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ClientExists(email))
                {
                    return NotFound();
                }
                else
                {
                                
                logger.LogError($"Something went wrong inside GetClient action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            
                }
            }

            return NoContent();
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            try
            {
                if (ClientExists(client.Email))
                {
                    return StatusCode(303, "User already Exist");
                }
                _context.Client.Add(client);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetClient", new { email = client.Email }, client);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PostClient action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(Client client)
        {
            try
            {
                if (ClientExists(client.Email))
                {
                    var date = DateTime.UtcNow;
                    var expireDate = TimeSpan.FromHours(5);
                    var expireDateTime = date.Add(expireDate);

                    var token = _authService.GenerateToken(date, client.UserName, expireDate);

                    return Ok(new
                    {
                        Token = token,
                        ExpireAt = expireDateTime
                    });

                }
                else
                {
                    return StatusCode(401, client.Email + " is not registered");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside PostClient action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool ClientExists(string email)
        {
            return _context.Client.Any(e => e.Email == email);
        }
    }
}
