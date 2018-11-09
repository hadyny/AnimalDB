using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IAdministrator
    {
        IEnumerable<Administrator> GetAdministrators();

        Task CreateAdministrator(Administrator administrator);

        Task<Administrator> GetAdministratorById(string id);

        Administrator GetAdministratorByUsername(string userName);

        Task UpdateAdministrator(Administrator administrator);

        Task DeleteAdministrator(Administrator administrator);

        Task SetAuthCookie(string userName);
    }
}
