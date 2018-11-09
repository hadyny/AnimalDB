using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface INotCheckedRoom
    {
        IEnumerable<NotCheckedRoom> GetNotCheckedRooms();

        Task CreateNotCheckedRoom(NotCheckedRoom notCheckedRoom);

        Task<NotCheckedRoom> GetNotCheckedRoomById(int id);

        Task UpdateNotCheckedRoom(NotCheckedRoom notCheckedRoom);

        Task DeleteNotCheckedRoom(NotCheckedRoom notCheckedRoom);
    }
}
