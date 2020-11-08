using FacesStorage.Data.Models;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRoleRepository : IRepository
    {
        string DefaultUserRole { get; set; }
        string DefaultAdminRole { get; set; }
        bool Any();
        Task<Role> GetAsync(string name);
        Task<Role> CreateAsync(Role role);
    }
}
