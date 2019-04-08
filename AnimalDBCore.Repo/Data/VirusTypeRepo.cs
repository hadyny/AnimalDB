using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using AnimalDBCore.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class VirusTypeRepo : IVirusType, IDisposable
    {
        private AnimalDBContext db;

        public VirusTypeRepo()
        {
            this.db = new AnimalDBContext();
        }

        public VirusTypeRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateVirusType(VirusType virusType)
        {
            db.VirusTypes.Add(virusType);
            await db.SaveChangesAsync();
        }

        public async Task DeleteVirusType(VirusType virusType)
        {
            if (db.Entry(virusType).State == EntityState.Detached)
            {
                db.VirusTypes.Attach(virusType);
            }
            db.VirusTypes.Remove(virusType);
            await db.SaveChangesAsync();
        }

        public async Task<VirusType> GetVirusTypeById(int id)
        {
            return await db.VirusTypes.FindAsync(id);
        }

        public IEnumerable<VirusType> GetVirusTypes()
        {
            return db.VirusTypes.ToList();
        }

        public async Task UpdateVirusType(VirusType VirusType)
        {
            db.Entry(VirusType).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
