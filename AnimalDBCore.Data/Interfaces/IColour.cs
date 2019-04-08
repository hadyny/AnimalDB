using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
