using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<Role> All()
        {
            return roleDbSet.AsQueryable();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            Role role = await roleDbSet.FirstOrDefaultAsync(r => r.RoleId == id);
            if (role == null) throw new KeyNotFoundException($"Not found role with id equal {id}.");

            return role;
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            Role role = await roleDbSet.FirstOrDefaultAsync(r => r.Name == name);
            if (role == null) throw new KeyNotFoundException($"Not found role with name equal {name}.");

            return role;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            var entityEntry = await roleDbSet.AddAsync(role);
            return entityEntry.Entity;
        }

        public Role Edit(Role role)
        {
            var entityEntry = roleDbSet.Update(role);
            return entityEntry.Entity;
        }

        public void Delete(Role role)
        {
            roleDbSet.Remove(role);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.roleDbSet = this.storageContext.Set<Role>();
        }
    }
}
