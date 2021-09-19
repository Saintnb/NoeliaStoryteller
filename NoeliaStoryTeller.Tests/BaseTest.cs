using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeliaStoryTeller.Tests
{
   public class BaseTest
    {
        protected DbContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(dbName).Options;

            var dbContext = new DbContext(options);
            return dbContext;
        }
    }
}
