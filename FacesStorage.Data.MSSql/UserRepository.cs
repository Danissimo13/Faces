using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class UserRepository : IUserRepository
    {
        private StorageContext storageContext;
        private DbSet<User> userDbSet;

        public IQueryable<User> All()
        {
            return userDbSet.AsQueryable<User>();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await userDbSet
                .Include(u => u.Role)
                .Include(u => u.Requests)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) throw new KeyNotFoundException($"Not found user with id equal {id}.");

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {

            User user = await userDbSet
                .Include(u => u.Role)
                .Include(u => u.Requests)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) throw new KeyNotFoundException($"Not found user with email equal {email}.");
            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            var userWithSameEmailOrNick = await userDbSet.FirstOrDefaultAsync(u => (u.Email == user.Email) || (u.Nickname == user.Nickname));
            if (userWithSameEmailOrNick != null)
            {
                throw new DuplicateNameException($"User with this email: {user.Email}, already exist.");
            }

            var entityEntry = await userDbSet.AddAsync(user);
            return entityEntry.Entity;
        }

        public User Edit(User user)
        {
            var entityEntry = userDbSet.Update(user);
            return entityEntry.Entity;
        }

        public void Delete(User user)
        {
            userDbSet.Remove(user);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.userDbSet = this.storageContext.Set<User>();
        }
    }
}