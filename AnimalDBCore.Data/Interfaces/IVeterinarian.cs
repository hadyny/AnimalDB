using System.Collections.Generic;
using System.Threading.Tasks;

namespace  AnimalDBCore.Core.Interfaces
{
    public interface IVeterinarian<T>
    {
        IEnumerable<T> GetVeterinarians();

        T GetVeterinarianByUsername(string username);

        Task CreateVeterinarian(T veterinarian);

        Task<T> GetVeterinarianById(string id);

        Task UpdateVeterinarian(T veterinarian);

        Task DeleteVeterinarian(T veterinarian);

        Task SetAuthCookie(string userName);
    }
}
