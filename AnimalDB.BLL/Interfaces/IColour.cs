using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IColour
    {
        IEnumerable<Colour> GetColours();

        Task CreateColour(Colour colour);

        Task<Colour> GetColourById(int id);

        Task UpdateColour(Colour colour);

        Task DeleteColour(Colour colour);
    }
}
