using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
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
