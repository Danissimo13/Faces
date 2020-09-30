using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class RequestImageRepository : IRequestImageRepository
    {
        private StorageContext storageContext;
        private DbSet<RequestImage> requestImagesDbSet;

        public async Task<RequestImage> CreateAsync(RequestImage image)
        {
            var entityEntry = await requestImagesDbSet.AddAsync(image);
            return entityEntry.Entity;
        }

        public RequestImage Edit(RequestImage image)
        {
            var entityEntry = requestImagesDbSet.Update(image);
            return entityEntry.Entity;
        }

        public void Delete(RequestImage image)
        {
            requestImagesDbSet.Remove(image);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.requestImagesDbSet = this.storageContext.Set<RequestImage>();
        }
    }
}
