using NoeliaStorytellerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.DTO
{
    public class MessageItemDTO
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string Email { get; set; }

        public MessageItemDTO () { }
        
        public MessageItemDTO( MessageItem m, Client c)
        {
            this.Message = m.Message;
            this.UserName = c.UserName;
            //this.Email = m.Email;
            this.CreatedDate = m.CreationDate;
        }

    }
}
