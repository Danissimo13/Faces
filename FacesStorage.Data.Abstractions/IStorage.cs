﻿using System.Threading.Tasks;

namespace FacesStorage.Data.Abstractions
{
    public interface IStorage
    {
        T GetRepository<T>() where T : IRepository;
        void Save();
        Task SaveAsync();
    }
}
