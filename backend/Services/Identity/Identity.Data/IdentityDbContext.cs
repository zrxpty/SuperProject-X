﻿using Identity.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
    }
}