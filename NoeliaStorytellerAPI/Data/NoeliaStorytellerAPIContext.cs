using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoeliaStorytellerAPI.Models;

namespace NoeliaStorytellerAPI.Data
{
    public class NoeliaStorytellerAPIContext : DbContext
    {
        public NoeliaStorytellerAPIContext (DbContextOptions<NoeliaStorytellerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NoeliaStorytellerAPI.Models.MessageItem> MessageItem { get; set; }

        public DbSet<NoeliaStorytellerAPI.Models.Client> Client { get; set; }
    }
}
