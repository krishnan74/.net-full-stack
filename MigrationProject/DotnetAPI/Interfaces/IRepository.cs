using System;
using System.Collections.Generic;

namespace DotnetAPI.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        public Task<T> Add(T item);
        public Task<T> Get(K key);
        public Task<IEnumerable<T>> GetAll( 
            int pageNumber = 1,
            int pageSize = 10
        );
        public Task<T> Update(K key, T item);
        public Task<T> Delete(K key);
    }
}