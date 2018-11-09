using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IAnimalRoomCount
    {
        IEnumerable<AnimalRoomCount> GetAnimalRoomCounts();

        Task CreateAnimalRoomCount(AnimalRoomCount animalRoomCount);

        Task<AnimalRoomCount> GetAnimalRoomCountById(int id);

        Task UpdateAnimalRoomCount(AnimalRoomCount animalRoomCount);

        Task DeleteAnimalRoomCount(AnimalRoomCount animalRoomCount);

        IEnumerable<AnimalRoomCount> GetLastNRoomCountsByRoomId(int roomId, int amount = 30);
    }
}
