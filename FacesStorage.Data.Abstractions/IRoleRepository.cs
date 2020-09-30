using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        string DefaultUserRole { get; set; }
        string DefaultAdminRole { get; set; }

        IQueryable<Role> All();
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
        Task<Role> CreateAsync(Role role);
        Role Edit(Role role);
        void Delete(Role role);
    }
}
