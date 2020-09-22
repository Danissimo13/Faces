using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FacesStorage.Data.MSSql
{
    public class StorageContext : DbContext, IStorageContext
    {
        private readonly string connectionString;

        public StorageContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(etb =>
            {
                etb.HasKey(e => e.UserId);
                etb.Property(e => e.UserId);
                etb.Property(e => e.Email);
                etb.Property(e => e.PasswordId);
                etb.Property(e => e.RoleId);
                etb.ToTable("Users");
            });
            modelBuilder.Entity<User>().HasOne(e => e.Password).WithOne().HasForeignKey<User>(e => e.PasswordId);
            modelBuilder.Entity<User>().HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId);
        }
    }
}
