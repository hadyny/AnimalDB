using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<T> GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        Task Delete(object id);
        Task Save();
    }
}
