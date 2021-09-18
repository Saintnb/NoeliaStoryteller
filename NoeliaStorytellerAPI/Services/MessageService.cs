using Microsoft.Extensions.Logging;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.DTO;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly NoeliaStorytellerAPIContext _context;
        private readonly ILogger<MessageService> _logger;

        public MessageService( ILogger<MessageService> logger)
        {            
            this._logger = logger;
        }
        public List<MessageItemDTO> MapMessages(List<MessageItem> messages )
        {
            List<MessageItemDTO> mdto = new();
            try
            {
                messages.ForEach(async x =>
                {
                    Client c = await _context.Client.FindAsync(x.Email);
                    MessageItemDTO m = new MessageItemDTO(x, c);
                    mdto.Add(m);
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside MapMessages action: {ex.Message}");
                throw;
            }
            return mdto;
        }
    }
}
