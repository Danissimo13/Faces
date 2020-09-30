using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class RequestRepository<T> : IRequestRepository<T> where T : Request
    {
        private StorageContext storageContext;
        private DbSet<T> requestDbSet;
        private DbSet<RequestImage> requestImagesDbSet;

        public IQueryable<T> All()
        {
            return requestDbSet.AsQueryable<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T request = await requestDbSet
                .Include(r => r.User)
                .Include(r => r.Response)
                .FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null) throw new KeyNotFoundException($"Not found request with id equal {id}.");

            await requestImagesDbSet.Where(i => i.RequestId == request.RequestId).LoadAsync(); // loading associated images

            return request;
        }

        public async Task<T> CreateAsync(T request)
        {
            var entityEntry = await requestDbSet.AddAsync(request);
            return entityEntry.Entity;
        }

        public T Edit(T request)
        {
            var entityEntry = requestDbSet.Update(request);
            return entityEntry.Entity;
        }

        public void Delete(T request)
        {
            requestDbSet.Remove(request);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.requestDbSet = this.storageContext.Set<T>();
            this.requestImagesDbSet = this.storageContext.Set<RequestImage>();
        }
    }
}
