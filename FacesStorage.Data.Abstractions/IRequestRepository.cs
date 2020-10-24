using FacesStorage.Data.Abstractions.SearchOptions;
using FacesStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IRequestRepository : IRepository
    {
        public IEnumerable<Request> All(Action<RequestsSearchOptions> optionsBuilder);
        public Task<Request> GetAsync(Action<RequestSearchOptions> optionsBuilder);
        public Task<Request> CreateAsync(Request request);
        public Request Edit(Request request);
        public void Delete(Request request);
    }
}
