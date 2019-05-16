using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IAdministratorService
    {
        Task <IEnumerable<Administrator>> GetAdministrators();

        Task CreateAdministrator(Administrator administrator);

        Task<Administrator> GetAdministratorById(string id);

        Task <Administrator> GetAdministratorByUsername(string userName);

        Task UpdateAdministrator(Administrator administrator);

        Task DeleteAdministrator(Administrator administrator);

        Task SetAuthCookie(string userName);
    }
}
