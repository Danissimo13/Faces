using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class ResponseRepository : IResponseRepository
    {
        private StorageContext storageContext;
        private DbSet<Response> responseDbSet;

        public IQueryable<TResponse> All<TResponse>() where TResponse : Response
        {
            var responsesByType = storageContext.Set<TResponse>();
            return responsesByType.AsQueryable();
        }

        public async Task<Response> GetByIdAsync(int id)
        {
            Response response = await responseDbSet
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.ResponseId == id);
            if(response == null) throw new KeyNotFoundException($"Not found response with id equal {id}.");

            return response;
        }

        public async Task<Response> CreateAsync(Response response)
        {
            var entityEntry = await responseDbSet.AddAsync(response);
            return entityEntry.Entity;
        }

        public Response Edit(Response response)
        {
            var entityEntry = responseDbSet.Update(response);
            return entityEntry.Entity;
        }

        public void Delete(Response response)
        {
            responseDbSet.Remove(response);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.responseDbSet = this.storageContext.Set<Response>();
        }
    }
}
