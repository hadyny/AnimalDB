using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDBCore.Core.Interfaces
{
    public interface ITechnician<T>
    {
        IEnumerable<T> GetTechnicians();

        T GetTechnicianByUsername(string username);

        Task CreateTechnician(T technician);

        Task<T> GetTechnicianById(string id);

        Task UpdateTechnician(T technician);

        Task DeleteTechnician(T technician);

        Task SetAuthCookie(string userName);
    }
}
