using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{ 
    public interface IUserRepository : IRepository
    {
        IQueryable<User> All();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        User Edit(User user);
        void Delete(User user);
    }
}
