using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class RackRepo : IRack, IDisposable
    {
        private AnimalDBContext db;

        public RackRepo()
        {
            this.db = new AnimalDBContext();
        }

        public RackRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateRack(Rack rack)
        {
            db.Racks.Add(rack);
            await db.SaveChangesAsync();
        }

        public async Task DeleteRack(Rack rack)
        {
            if (db.Entry(rack).State == EntityState.Detached)
            {
                db.Racks.Attach(rack);
            }
            db.Racks.Remove(rack);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Rack> GetRackById(int id)
        {
            return await db.Racks.FindAsync(id);
        }

        public IEnumerable<Rack> GetRacks()
        {
            return db.Racks.ToList();
        }

        public async Task UpdateRack(Rack rack)
        {
            db.Entry(rack).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<Rack> GetRacksByRoomId(int roomId)
        {
            return db.Racks.Where(m => m.Room_Id == roomId).ToList();
        }
    }
}
