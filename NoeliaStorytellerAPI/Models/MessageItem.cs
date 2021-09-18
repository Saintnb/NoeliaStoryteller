using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoeliaStorytellerAPI.Models
{
    public class MessageItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        [EmailAddress]
        public string Email{ get; set; }


    }
}
