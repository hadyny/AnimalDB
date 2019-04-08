using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class CageLocationRepo : ICageLocation
    {
        private readonly AnimalDBContext db;

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
