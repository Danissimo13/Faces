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
        public const string defaultUserRole = "user";
        public const string defaultAdminRole = "admin";

        private StorageContext storageContext;
        private DbSet<Role> roleDbSet;

        public IQueryable<Role> All()
        {
            return roleDbSet.AsQueryable();
        }

        public async Task<Role> GetById(int id)
        {
            Role role = await roleDbSet.FirstOrDefaultAsync(r => r.RoleId == id);
            if (role == null) throw new KeyNotFoundException($"Not found role with id equal {id}.");

            return role;
        }

        public async Task<Role> GetByName(string name)
        {
            Role role = await roleDbSet.FirstOrDefaultAsync(r => r.Name == name);
            if (role == null) throw new KeyNotFoundException($"Not found role with name equal {name}.");

            return role;
        }

        public async Task<Role> Create(Role role)
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
