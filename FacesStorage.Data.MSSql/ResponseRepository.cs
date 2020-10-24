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
    class ResponseRepository : IResponseRepository
    {
        private StorageContext storageContext;
        private DbSet<Response> responseDbSet;

        public async Task<Response> GetAsync(Action<ResponseSearchOptions> optionsBuilder)
        {
            ResponseSearchOptions searchOptions = new ResponseSearchOptions();
            optionsBuilder(searchOptions);

            var responses = responseDbSet.AsQueryable<Response>();
            if (searchOptions.WithImages)
                responses = responses.Include(r => r.Images);

            Response response = await responses.FirstOrDefaultAsync(r => r.ResponseId == searchOptions.ResponseId);
            if(response == null) throw new ResponseNotFoundException($"Not found response with id equal {searchOptions.ResponseId}.");

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
