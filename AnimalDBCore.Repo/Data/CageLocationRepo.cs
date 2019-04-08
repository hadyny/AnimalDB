using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class CageLocationRepo : ICageLocation, IDisposable
    {
        private AnimalDBContext db;

        public CageLocationRepo()
        {
            this.db = new AnimalDBContext();
        }
        public CageLocationRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateCageLocation(CageLocation cageLocation)
        {
            db.CageLocations.Add(cageLocation);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCageLocation(CageLocation cageLocation)
        {
            if (db.Entry(cageLocation).State == EntityState.Detached)
            {
                db.CageLocations.Attach(cageLocation);
            }
            db.CageLocations.Remove(cageLocation);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<CageLocation> GetCageLocationById(int id)
        {
            return await db.CageLocations.FindAsync(id);
        }

        public IEnumerable<CageLocation> GetCageLocations()
        {
            return db.CageLocations.ToList();
        }

        public async Task UpdateCageLocation(CageLocation cageLocation)
        {
            db.Entry(cageLocation).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
