using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ParrotWings.API.Models;

namespace ParrotWings.API
{
    public class ParrotWingsContext : DbContext
    {
        public ParrotWingsContext()
            :base("ParrotWingsDB")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.TransactionLogs)
                .WithRequired(e => e.Recipient)
                .HasForeignKey(e => e.RecipientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TransactionLogs)
                .WithRequired(e => e.Sender)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);
        }
    }
}