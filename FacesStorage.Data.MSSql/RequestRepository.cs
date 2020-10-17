using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class RequestRepository : IRequestRepository
    {
        private StorageContext storageContext;
        private DbSet<Request> requestDbSet;

        public IQueryable<TRequest> All<TRequest>() where TRequest : Request
        {
            var requestsByTypes = this.storageContext.Set<TRequest>();
            return requestsByTypes.AsQueryable<TRequest>();
        }

        public async Task<Request> GetByIdAsync(int id)
        {
            Request request = await requestDbSet
                .Include(r => r.User)
                .Include(r => r.Response)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null) throw new KeyNotFoundException($"Not found request with id equal {id}.");

            return request;
        }

        public async Task<Request> CreateAsync(Request request)
        {
            var entityEntry = await requestDbSet.AddAsync(request);
            return entityEntry.Entity;
        }

        public Request Edit(Request request)
        {
            var entityEntry = requestDbSet.Update(request);
            return entityEntry.Entity;
        }

        public void Delete(Request request)
        {
            requestDbSet.Remove(request);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.requestDbSet = this.storageContext.Set<Request>();
        }
    }
}
