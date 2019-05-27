using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class AnimalRoomCountService : IAnimalRoomCountService
    {
        private readonly IRepository<AnimalRoomCount> _roomCounts;

        public AnimalRoomCountService(IRepository<AnimalRoomCount> roomCounts)
        {
            this._roomCounts = roomCounts;
        }

        public async Task CreateAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            _roomCounts.Insert(animalRoomCount);
            await _roomCounts.Save();
        }

        public async Task DeleteAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            await _roomCounts.Delete(animalRoomCount.Id);
            await _roomCounts.Save();
        }

        public async Task<IEnumerable<AnimalRoomCount>> GetLastNRoomCountsByRoomId(int roomId, int amount = 30)
        {
            var counts = await _roomCounts.GetAll(m => m.Room_Id == roomId);
            return counts
                    .OrderByDescending(m => m.Timestamp)
                    .Take(amount);
        }

        public async Task<AnimalRoomCount> GetAnimalRoomCountById(int id)
        {
            return await _roomCounts.GetById(id);
        }

        public async Task<IEnumerable<AnimalRoomCount>> GetAnimalRoomCounts()
        {
            return await _roomCounts.GetAll();
        }

        public async Task UpdateAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            _roomCounts.Update(animalRoomCount);
            await _roomCounts.Save();
        }
    }
}
