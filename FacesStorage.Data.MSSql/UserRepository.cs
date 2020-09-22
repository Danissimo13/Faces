using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class UserRepository : IUserRepository
    {
        private StorageContext storageContext;
        private DbSet<User> dbSet;

        public IQueryable<User> All()
        {
            return dbSet.AsQueryable<User>();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await dbSet.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) throw new KeyNotFoundException($"Not found user with id equal {id}");

            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            var entityEntry = await dbSet.AddAsync(user);
            return entityEntry.Entity;
        }

        public User Edit(User user)
        {
            var entityEntry = dbSet.Update(user);
            return entityEntry.Entity;
        }

        public void Delete(User user)
        {
            dbSet.Remove(user);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.dbSet = this.storageContext.Set<User>();
        }
    }
}