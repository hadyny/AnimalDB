using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> _rooms;

        public RoomService(IRepository<Room> rooms)
        {
            this._rooms = rooms;
        }

        public async Task CreateRoom(Room room)
        {
            _rooms.Insert(room);
            await _rooms.Save();
        }

        public async Task<int> GetCountOfLivingAnimalsByRoomId(int roomId)
        {
            var room = await _rooms.GetById(roomId);
            return room
                    .Animals
                    .Count(m => m.DeathDate == null);
        }

        public async Task<int> GetCountOfGMOAnimalsByRoomId(int roomId)
        {
            var room = await _rooms.GetById(roomId);
            return room
                    .Animals
                    .Where(m => m.ApprovalNumber_Id != null && m.DeathDate == null)
                    .Count();
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimalsByRoomId(int roomId)
        {
            var room = await _rooms.GetById(roomId);
            return room.Animals.Where(m => m.DeathDate == null);
        }

        public async Task DeleteRoom(Room room)
        {
            await _rooms.Delete(room.Id);
            await _rooms.Save();
        }

        public async Task<Room> GetRoomById(int id)
        {
            return await _rooms.GetById(id);
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            return await _rooms.GetAll(); ;
        }

        public async Task UpdateRoom(Room room)
        {
            _rooms.Update(room);
            await _rooms.Save();
        }

        public async Task<IEnumerable<Room>> RoomsThatHaventBeenCheckedToday()
        {
            return await _rooms.GetAll(m =>
                            m.NoDBAnimals &&
                            (!m.NoDBAnimalsLastCheck.HasValue ||
                                (m.NoDBAnimalsLastCheck.HasValue &&
                                DbFunctions.TruncateTime(m.NoDBAnimalsLastCheck.Value) != DbFunctions.TruncateTime(DateTime.Now))));

        }

        public async Task MarkAllAnimalsAsCheckedByRoomId(int roomId)
        {
            var room = await _rooms.GetById(roomId);

            foreach (var animal in room.Animals.Where(m => m.DeathDate == null))
            {
                animal.LastChecked = DateTime.Now.Date;
            }

            await _rooms.Save();
        }

        public async Task MarkRoomAsChecked(int roomId)
        {
            var room = await _rooms.GetById(roomId);

            room.NoDBAnimalsLastCheck = DateTime.Now;
            await _rooms.Save();
        }
    }
}
