using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IVeterinarianService
    {
        Task<IEnumerable<Veterinarian>> GetVeterinarians();

        Task<Veterinarian> GetVeterinarianByUsername(string username);

        Task CreateVeterinarian(Veterinarian veterinarian);

        Task<Veterinarian> GetVeterinarianById(string id);

        Task UpdateVeterinarian(Veterinarian veterinarian);

        Task DeleteVeterinarian(Veterinarian veterinarian);

        Task SetAuthCookie(string userName);
    }
}
