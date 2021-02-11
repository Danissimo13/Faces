using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using System;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{ 
    public interface IUserRepository : IRepository
    {
        bool Any();
        Task<User> GetAsync(Action<UserSearchOptions> searchOptionsBuilder);
        Task<User> CreateAsync(User user);
        User Edit(User user);
        void Delete(User user);
    }
}
