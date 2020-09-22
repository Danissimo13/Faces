namespace FacesStorage.Data.Abstractions
{
    public interface IRepository
    {
        void SetStorageContext(IStorageContext storageContext);
    }
}
