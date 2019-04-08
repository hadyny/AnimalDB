﻿using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class CageLocationHistoryRepo : ICageLocationHistory
    {
        private readonly AnimalDBContext db;

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
