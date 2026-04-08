using Microsoft.EntityFrameworkCore;
using nxtool.Models;
using System.Collections.Generic;

namespace nxtool.Data
{
    public class NxToolContext : DbContext
    {
        public NxToolContext(DbContextOptions<NxToolContext> options) : base(options) { }

        public DbSet<TokenRecord> Tokens { get; set; }
    }
}
