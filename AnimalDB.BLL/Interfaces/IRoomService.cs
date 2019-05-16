using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRooms();

        Task CreateRoom(Room room);

        Task<Room> GetRoomById(int id);

        Task UpdateRoom(Room room);

        Task DeleteRoom(Room room);

        Task<IEnumerable<Room>> RoomsThatHaventBeenCheckedToday();

        Task<int> GetCountOfLivingAnimalsByRoomId(int roomId);

        Task<int> GetCountOfGMOAnimalsByRoomId(int roomId);

        Task<IEnumerable<Animal>> GetLivingAnimalsByRoomId(int roomId);

        Task MarkAllAnimalsAsCheckedByRoomId(int roomId);

        Task MarkRoomAsChecked(int roomId);
    }
}
