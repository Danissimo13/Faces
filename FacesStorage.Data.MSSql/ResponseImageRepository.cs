using FacesStorage.Data.Abstractions;
using FacesStorage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    class ResponseImageRepository : IResponseImageRepository
    {
        private StorageContext storageContext;
        private DbSet<ResponseImage> responseImagesDbSet;

        public async Task<ResponseImage> CreateAsync(ResponseImage image)
        {
            var entityEntry = await responseImagesDbSet.AddAsync(image);
            return entityEntry.Entity;
        }

        public ResponseImage Edit(ResponseImage image)
        {
            var entityEntry = responseImagesDbSet.Update(image);
            return entityEntry.Entity;
        }

        public void Delete(ResponseImage image)
        {
            responseImagesDbSet.Remove(image);
        }

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.responseImagesDbSet = this.storageContext.Set<ResponseImage>();
        }
    }
}
