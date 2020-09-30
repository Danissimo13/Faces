using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class ResponseRepository<T> : IResponseRepository<T> where T : Response
    {
        private StorageContext storageContext;
        private DbSet<T> responseDbSet;

        public IQueryable<T> All()
        {
            return responseDbSet.AsQueryable();
        }

        public async Task<T> GetById(int id)
        {
            T response = await responseDbSet
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.ResponseId == id);
            if(response == null) throw new KeyNotFoundException($"Not found response with id equal {id}.");

            return response;
        }

        public async Task<T> Create(T response)
        {
            var entityEntry = await responseDbSet.AddAsync(response);
            return entityEntry.Entity;
        }

        public T Edit(T response)
        {
            var entityEntry = responseDbSet.Update(response);
            return entityEntry.Entity;
        }

        public void Delete(T response)
        {
            responseDbSet.Remove(response);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.responseDbSet = this.storageContext.Set<T>();
        }
    }
}
