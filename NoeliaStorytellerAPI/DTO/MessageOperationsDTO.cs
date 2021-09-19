using NoeliaStorytellerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.DTO
{
    public class MessageOperationsDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }

        public Client client { get; set; }
    }
}
