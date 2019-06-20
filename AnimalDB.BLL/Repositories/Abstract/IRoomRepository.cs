using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<int> GetCountOfLivingAnimalsByRoomId(int roomId);
        Task<int> GetCountOfGMOAnimalsByRoomId(int roomId);
        Task<IEnumerable<Animal>> GetLivingAnimalsByRoomId(int roomId);
        IEnumerable<Room> NotCheckedToday();
        Task MarkAllAnimalsAsCheckedByRoomId(int roomId);
        Task MarkRoomAsChecked(int roomId);
    }
}
