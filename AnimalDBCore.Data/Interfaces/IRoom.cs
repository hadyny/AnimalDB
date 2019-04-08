using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IRoom
    {
        IEnumerable<Room> GetRooms();

        Task CreateRoom(Room room);

        Task<Room> GetRoomById(int id);

        Task UpdateRoom(Room room);

        Task DeleteRoom(Room room);

        IEnumerable<Room> RoomsThatHaventBeenCheckedToday();

        int GetCountOfLivingAnimalsByRoomId(int roomId);

        int GetCountOfGMOAnimalsByRoomId(int roomId);

        IEnumerable<Animal> GetLivingAnimalsByRoomId(int roomId);

        Task MarkAllAnimalsAsCheckedByRoomId(int roomId);

        Task MarkRoomAsChecked(int roomId);
    }
}
