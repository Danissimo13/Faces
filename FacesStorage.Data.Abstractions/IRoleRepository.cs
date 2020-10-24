using FacesStorage.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        string DefaultUserRole { get; set; }
        string DefaultAdminRole { get; set; }
        Task<Role> GetAsync(string name);
        Task<Role> CreateAsync(Role role);
        Role Edit(Role role);
        void Delete(Role role);
    }
}
