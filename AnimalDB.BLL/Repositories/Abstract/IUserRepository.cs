using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IUserRepository<T> where T : AnimalUser
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(object id);
        Task<T> GetByUsername(string name);
        Task Insert(T obj, Repo.Enums.UserType userType);
        void Update(T obj);
        Task Delete(T obj);
    }
}
