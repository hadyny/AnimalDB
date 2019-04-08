using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDBCore.Core.Interfaces
{
    public interface IAdministrator<T>
    {
        IEnumerable<T> GetAdministrators();

        Task CreateAdministrator(T administrator);

        Task<T> GetAdministratorById(string id);

        T GetAdministratorByUsername(string userName);

        Task UpdateAdministrator(T administrator);

        Task DeleteAdministrator(T administrator);

        Task SetAuthCookie(string userName);
    }
}
