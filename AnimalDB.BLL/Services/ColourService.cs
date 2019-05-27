using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class ColourService : IColourService
    {
        private readonly IRepository<Colour> _colours;

        public ColourService(IRepository<Colour> colours)
        {
            this._colours = colours;
        }

        public async Task CreateColour(Colour colour)
        {
            _colours.Insert(colour);
            await _colours.Save();
        }

        public async Task DeleteColour(Colour colour)
        {
            await _colours.Delete(colour.Id);
            await _colours.Save();
        }

        public async Task<Colour> GetColourById(int id)
        {
            return await _colours.GetById(id);
        }

        public async Task<IEnumerable<Colour>> GetColours()
        {
            return await _colours.GetAll();
        }

        public async Task UpdateColour(Colour colour)
        {
            _colours.Update(colour);
            await _colours.Save();
        }
    }
}
