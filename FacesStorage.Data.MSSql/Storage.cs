using FacesStorage.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FacesStorage.Data.MSSql
{
    public class Storage : IStorage
    {
        public StorageContext StorageContext { get; private set; }

        public Storage(string connectionString)
        {
            StorageContext = new StorageContext(connectionString);
        }

        public T GetRepository<T>() where T : IRepository
        {
            foreach(Type type in this.GetType().GetTypeInfo().Assembly.GetTypes())
            {
                if(typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
                {
                    T repository = (T)Activator.CreateInstance(type);

                    repository.SetStorageContext(StorageContext);
                    return repository;
                }
            }

            return default(T);
        }

        public void Save()
        {
            StorageContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await StorageContext.SaveChangesAsync();
        }
    }
}
