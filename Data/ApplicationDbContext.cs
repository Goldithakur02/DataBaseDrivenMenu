using System;
using System.Collections.Generic;
using System.Text;
using MenuExample.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MenuExample.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public DbSet<Menu> Menu { get; set; }
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }
    }
}