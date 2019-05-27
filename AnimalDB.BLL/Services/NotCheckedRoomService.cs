using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class NotCheckedRoomService : INotCheckedRoomService
    {
        private readonly IRepository<NotCheckedRoom> _notCheckedRooms;

        public NotCheckedRoomService(IRepository<NotCheckedRoom> notCheckedRooms)
        {
            this._notCheckedRooms = notCheckedRooms;
        }

        public async Task CreateNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            _notCheckedRooms.Insert(notCheckedRoom);
            await _notCheckedRooms.Save();
        }

        public async Task DeleteNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            await _notCheckedRooms.Delete(notCheckedRoom.Id);
            await _notCheckedRooms.Save();
        }

        public async Task<NotCheckedRoom> GetNotCheckedRoomById(int id)
        {
            return await _notCheckedRooms.GetById(id);
        }

        public async Task<IEnumerable<NotCheckedRoom>> GetNotCheckedRooms()
        {
            return await _notCheckedRooms.GetAll();
        }

        public async Task UpdateNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            _notCheckedRooms.Update(notCheckedRoom);
            await _notCheckedRooms.Save();
        }
    }
}
