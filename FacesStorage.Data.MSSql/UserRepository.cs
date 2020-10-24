using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Abstractions.Exceptions;
using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public IList<User> All(Action<UsersSearchOptions> searchOptionsBuilder)
        {
            UsersSearchOptions searchOptions = new UsersSearchOptions();
            searchOptionsBuilder(searchOptions);

            IQueryable<User> users = userDbSet.AsQueryable<User>();

            if (searchOptions.From.HasValue)
                users = users.Skip(searchOptions.From.Value);
            if (searchOptions.Count.HasValue)
                users = users.Take(searchOptions.Count.Value);

            if (searchOptions.WithRole)
                users = users.Include(u => u.Role);
            if (searchOptions.WithRequests)
                users = users.Include(u => u.Requests);
            if (searchOptions.WithRequestImages)
                users = users.Include(u => u.Requests).ThenInclude(r => r.Images);
            if (searchOptions.WithRequestResponses)
                users = users.Include(u => u.Requests).ThenInclude(r => r.Response);
            if (searchOptions.WithResponseImages)
                users = users.Include(u => u.Requests).ThenInclude(r => r.Response).ThenInclude(r => r.Images);

            users = users.Select(u => new User()
            {
                UserId = u.UserId,
                Email = u.Email,
                Nickname = u.Nickname,
                Role = u.Role,
                Requests = u.Requests,
                Password = searchOptions.WithPassword ? u.Password : null
            });

            switch (searchOptions.SearchType)
            {
                case UsersSearchTypes.ByEmail:
                    users = users.Where(u => u.Email.Contains(searchOptions.Email));
                    break;
                case UsersSearchTypes.ByNickname:
                    users = users.Where(u => u.Nickname.Contains(searchOptions.Nickname));
                    break;
                case UsersSearchTypes.WithoutProperty:
                    break;
            }
            
            var usersList = users.AsEnumerable().Select(u =>
            {
                if(searchOptions.FromRequest.HasValue)
                    u.Requests = u.Requests.OrderByDescending(r => r.RequestId).Skip(searchOptions.FromRequest.Value).ToList();
                if (searchOptions.RequestsCount.HasValue)
                    u.Requests = u.Requests.OrderByDescending(r => r.RequestId).Take(searchOptions.RequestsCount.Value).ToList();
                return u;
            }).ToList();

            return usersList;
        }

        public async Task<User> GetAsync(Action<UserSearchOptions> searchOptionsBuilder)
        {
            UserSearchOptions searchOptions = new UserSearchOptions();
            searchOptionsBuilder(searchOptions);

            IQueryable<User> users = userDbSet.AsQueryable<User>();

            if (searchOptions.WithRole) 
                users = users.Include(u => u.Role);
            if (searchOptions.WithRequests) 
                users = users.Include(u => u.Requests);
            if (searchOptions.WithRequestImages) 
                users = users.Include(u => u.Requests).ThenInclude(r => r.Images);
            if (searchOptions.WithRequestResponses) 
                users = users.Include(u => u.Requests).ThenInclude(r => r.Response);
            if (searchOptions.WithResponseImages) 
                users = users.Include(u => u.Requests).ThenInclude(r => r.Response).ThenInclude(r => r.Images);

            users = users.Select(u => new User()
            {
                UserId = u.UserId,
                Email = u.Email,
                Nickname = u.Nickname,
                Role = u.Role,
                Requests = u.Requests,
                Password = searchOptions.WithPassword ? u.Password : null
            });

            User user = null;
            switch (searchOptions.SearchType)
            {
                case UserSearchTypes.ById:
                    user = await users.FirstOrDefaultAsync(u => u.UserId == searchOptions.UserId);
                    if (user == null) throw new UserNotFoundException($"Not found user with id equal {searchOptions.UserId}.");
                    break;
                case UserSearchTypes.ByEmail:
                    user = await users.FirstOrDefaultAsync(u => u.Email == searchOptions.Email);
                    if (user == null) throw new UserNotFoundException($"Not found user with email equal {searchOptions.Email}.");
                    break;
                case UserSearchTypes.ByNickname:
                    user = await users.FirstOrDefaultAsync(u => u.Nickname == searchOptions.Nickname);
                    if (user == null) throw new UserNotFoundException($"Not found user with nickname equal {searchOptions.Nickname}.");
                    break;
                case UserSearchTypes.WithoutProperty:
                    break;
            }

            if(searchOptions.FromRequest.HasValue)
                user.Requests = user.Requests.OrderByDescending(r => r.RequestId).Skip(searchOptions.FromRequest.Value).ToList();
            if (searchOptions.RequestsCount.HasValue)
                user.Requests = user.Requests.OrderByDescending(r => r.RequestId).Take(searchOptions.RequestsCount.Value).ToList();

            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            var userWithSameEmailOrNick = await userDbSet.FirstOrDefaultAsync(u => (u.Email == user.Email) || (u.Nickname == user.Nickname));
            if (userWithSameEmailOrNick != null)
            {
                throw new UserAlreadyExistException($"User with this email: {user.Email}, already exist.");
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