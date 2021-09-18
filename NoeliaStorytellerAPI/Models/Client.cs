using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Models
{
    public class Client
    {

        [Key]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }


    }
}
