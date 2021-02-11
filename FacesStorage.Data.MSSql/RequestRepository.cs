using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Abstractions.Exceptions;
using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class RequestRepository : IRequestRepository
    {
        private StorageContext storageContext;
        private DbSet<Request> requestDbSet;

        public async Task<Request> GetAsync(Action<RequestSearchOptions> optionsBuilder)
        {
            RequestSearchOptions searchOptions = new RequestSearchOptions();
            optionsBuilder(searchOptions);

            var requests = requestDbSet.AsQueryable<Request>();
            if (searchOptions.WithUser)
                requests = requests.Include(r => r.User);
            if (searchOptions.WithImages)
                requests = requests.Include(r => r.Images);
            if (searchOptions.WithResponse)
                requests = requests.Include(r => r.Response);
            if (searchOptions.WithResponseImages)
                requests = requests.Include(r => r.Response).ThenInclude(r => r.Images);


            Request request = await requests.FirstOrDefaultAsync(r => r.RequestId == searchOptions.RequestId);
            if (request == null) throw new RequestNotFoundException($"Not found request with id equal {searchOptions.RequestId}.");

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
