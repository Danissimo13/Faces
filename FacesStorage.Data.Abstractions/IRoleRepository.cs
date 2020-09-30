using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        IQueryable<Role> All();
        Task<Role> GetById(int id);
        Task<Role> GetByName(string name);
        Task<Role> Create(Role role);
        Role Edit(Role role);
        void Delete(Role role);
    }
}
