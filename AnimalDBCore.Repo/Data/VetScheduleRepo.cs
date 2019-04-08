using AnimalDBCore.Infrastructure.Contexts;
using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class VetScheduleRepo : IVetSchedule, IDisposable
    {
        private AnimalDBContext db;

        public VetScheduleRepo()
        {
            this.db = new AnimalDBContext();
        }
        public VetScheduleRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateVetSchedule(VetSchedule VetSchedule)
        {
            db.VetSchedules.Add(VetSchedule);
            await db.SaveChangesAsync();
        }

        public async Task DeleteVetSchedule(VetSchedule VetSchedule)
        {
            if (db.Entry(VetSchedule).State == EntityState.Detached)
            {
                db.VetSchedules.Attach(VetSchedule);
            }
            db.VetSchedules.Remove(VetSchedule);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<VetSchedule> GetVetScheduleById(int id)
        {
            return await db.VetSchedules.FindAsync(id);
        }

        public IEnumerable<VetSchedule> GetVetSchedules()
        {
            return db.VetSchedules.ToList();
        }

        public async Task UpdateVetSchedule(VetSchedule VetSchedule)
        {
            db.Entry(VetSchedule).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
