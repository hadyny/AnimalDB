using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class NotCheckedRoomRepo : INotCheckedRoom, IDisposable
    {
        private AnimalDBContext db;

        public NotCheckedRoomRepo()
        {
            this.db = new AnimalDBContext();
        }

        public NotCheckedRoomRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            db.NotCheckedRooms.Add(notCheckedRoom);
            await db.SaveChangesAsync();
        }

        public async Task DeleteNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            if (db.Entry(notCheckedRoom).State == EntityState.Detached)
            {
                db.NotCheckedRooms.Attach(notCheckedRoom);
            }
            db.NotCheckedRooms.Remove(notCheckedRoom);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<NotCheckedRoom> GetNotCheckedRoomById(int id)
        {
            return await db.NotCheckedRooms.FindAsync(id);
        }

        public IEnumerable<NotCheckedRoom> GetNotCheckedRooms()
        {
            return db.NotCheckedRooms.ToList();
        }

        public async Task UpdateNotCheckedRoom(NotCheckedRoom notCheckedRoom)
        {
            db.Entry(notCheckedRoom).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
