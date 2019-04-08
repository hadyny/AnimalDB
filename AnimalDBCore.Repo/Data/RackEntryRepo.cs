﻿using AnimalDBCore.Infrastructure.Contexts;
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
    public class RackEntryRepo : IRackEntry, IDisposable
    {
        private AnimalDBContext db;

        public RackEntryRepo()
        {
            this.db = new AnimalDBContext();
        }

        public RackEntryRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateRackEntry(RackEntry rackEntry)
        {
            db.RackEntries.Add(rackEntry);
            await db.SaveChangesAsync();
        }

        public async Task DeleteRackEntry(RackEntry rackEntry)
        {
            if (db.Entry(rackEntry).State == EntityState.Detached)
            {
                db.RackEntries.Attach(rackEntry);
            }
            db.RackEntries.Remove(rackEntry);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<RackEntry> GetRackEntryById(int id)
        {
            return await db.RackEntries.FindAsync(id);
        }

        public IEnumerable<RackEntry> GetRackEntriesByAnimalId(int animalId)
        {
            return db.RackEntries.Where(m => m.Animal_Id == animalId).ToList();
        }

        public IEnumerable<RackEntry> GetRackEntries()
        {
            return db.RackEntries.ToList();
        }

        public async Task UpdateRackEntry(RackEntry rackEntry)
        {
            db.Entry(rackEntry).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}