using NoeliaStorytellerAPI.DTO;
using NoeliaStorytellerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services.Messages
{
   public interface IMessageService
    {
         List<MessageItemDTO> MapMessages(List<MessageItem> messages);
    }
}
