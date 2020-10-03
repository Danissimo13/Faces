using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{ 
    public interface IUserRepository : IRepository
    {
        IList<User> All(Action<UsersSearchOptions> searchOptionsBuilder);
        Task<User> GetAsync(Action<UserSearchOptions> searchOptionsBuilder);
        Task<User> CreateAsync(User user);
        User Edit(User user);
        void Delete(User user);
    }
}
