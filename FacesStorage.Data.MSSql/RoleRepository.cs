using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Abstractions.Exceptions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class RoleRepository : IRoleRepository
    {
        private StorageContext storageContext;
        private DbSet<Role> roleDbSet;

        public string DefaultUserRole { get; set; }
        public string DefaultAdminRole { get; set; }

        public RoleRepository()
        {
            DefaultUserRole = "user";
            DefaultAdminRole = "admin";
        }

        public bool Any()
        {
            return roleDbSet.Any();
        }

        public async Task<Role> GetAsync(string name)
        {
            Role role = await roleDbSet.FirstOrDefaultAsync(r => r.Name == name);
            if (role == null) throw new RoleNotFoundException($"Not found role with name equal {name}.");

            return role;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            var entityEntry = await roleDbSet.AddAsync(role);
            return entityEntry.Entity;
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.roleDbSet = this.storageContext.Set<Role>();
        }
    }
}
