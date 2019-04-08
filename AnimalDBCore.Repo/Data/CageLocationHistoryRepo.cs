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
    public class CageLocationHistoryRepo : ICageLocationHistory, IDisposable
    {
        private AnimalDBContext db;

        public CageLocationHistoryRepo()
        {
            this.db = new AnimalDBContext();
        }
        public CageLocationHistoryRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            db.CageLocationHistories.Add(cageLocationHistory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            if (db.Entry(cageLocationHistory).State == EntityState.Detached)
            {
                db.CageLocationHistories.Attach(cageLocationHistory);
            }
            db.CageLocationHistories.Remove(cageLocationHistory);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public IEnumerable<CageLocationHistory> GetCageLocationHistories()
        {
            return db.CageLocationHistories.ToList();
        }

        public async Task<CageLocationHistory> GetCageLocationHistoryById(int id)
        {
            return await db.CageLocationHistories.FindAsync(id);
        }

        public IEnumerable<CageLocationHistory> GetCageLocationHistoryByAnimalId(int animalId)
        {

            return db.CageLocationHistories
                            .Include(c => c.Animal)
                            .Include(c => c.CageLocation)
                            .Where(m => m.Animal_Id == animalId)
                            .OrderByDescending(m => m.Timestamp)
                            .ToList();
        }


        public async Task UpdateCageLocationHistory(CageLocationHistory cageLocationHistory)
        {
            db.Entry(cageLocationHistory).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
