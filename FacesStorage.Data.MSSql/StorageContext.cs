using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FacesStorage.Data.MSSql
{
    public class StorageContext : DbContext, IStorageContext
    {
        private readonly string connectionString;

        public StorageContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
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
                etb.Property(e => e.Password);
                etb.HasOne(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.RoleId);
                etb.HasMany(e => e.Requests).WithOne(e => e.User).HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Role>(etb =>
            {
                etb.HasKey(e => e.RoleId);
                etb.Property(e => e.Name);
                etb.Property(e => e.RoleId);
                etb.HasMany(e => e.Users).WithOne(e => e.Role).HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<News>(etb =>
            {
                etb.HasKey(e => e.NewsId);
                etb.Property(e => e.NewsId);
                etb.Property(e => e.Topic);
                etb.Property(e => e.Body);
                etb.Property(e => e.ImagePath);
                etb.Property(e => e.PublishDate);
            });

            modelBuilder.Entity<Request>(etb =>
            {
                etb.HasKey(e => e.RequestId);
                etb.Property(e => e.ResponseId);
                etb.Property(e => e.UserId);
                etb.Property(e => e.Discriminator);
                etb.HasDiscriminator(e => e.Discriminator);
                etb.HasOne(e => e.Response).WithOne().HasForeignKey<Request>(e => e.ResponseId);
                etb.HasOne(e => e.User).WithMany(e => e.Requests).HasForeignKey(e => e.UserId);
                etb.HasMany(e => e.Images).WithOne(i => i.Request).HasForeignKey(e => e.RequestId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DetectRequest>(etb =>
            {
                etb.HasBaseType(typeof(Request));
            });

            modelBuilder.Entity<CutRequest>(etb =>
            {
                etb.HasBaseType(typeof(Request));
            });

            modelBuilder.Entity<SwapRequest>(etb =>
            {
                etb.HasBaseType(typeof(Request));
            });

            modelBuilder.Entity<RequestImage>(etb =>
            {
                etb.HasKey(e => e.ImageId);
                etb.Property(e => e.ImageId);
                etb.Property(e => e.ImageName);
                etb.Property(e => e.RequestId);
                etb.HasOne(e => e.Request).WithMany(r => r.Images).HasForeignKey(e => e.RequestId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Response>(etb =>
            {
                etb.HasKey(e => e.ResponseId);
                etb.Property(e => e.ResponseId);
                etb.Property(e => e.Discriminator);
                etb.HasDiscriminator(e => e.Discriminator);
                etb.HasMany(e => e.Images).WithOne(e => e.Response).HasForeignKey(e => e.ResponseId);
            });

            modelBuilder.Entity<DetectResponse>(etb =>
            {
                etb.HasBaseType(typeof(Response));
            });

            modelBuilder.Entity<CutResponse>(etb =>
            {
                etb.HasBaseType(typeof(Response));
            });

            modelBuilder.Entity<SwapResponse>(etb =>
            {
                etb.HasBaseType(typeof(Response));
            });

            modelBuilder.Entity<ResponseImage>(etb =>
            {
                etb.HasKey(e => e.ImageId);
                etb.Property(e => e.ImageId);
                etb.Property(e => e.ImageName);
                etb.Property(e => e.ResponseId);
                etb.HasOne(e => e.Response).WithMany(e => e.Images).HasForeignKey(e => e.ResponseId);
            });
            
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.BaseType == null)
                {
                    entity.SetTableName(entity.DisplayName());
                }
            }
        }
    }
}
