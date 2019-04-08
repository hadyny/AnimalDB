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
    public class SopRepo : ISop, IDisposable
    {
        private AnimalDBContext db;

        public SopRepo()
        {
            this.db = new AnimalDBContext();
        }

        public SopRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSop(Sop sop)
        {
            db.Sops.Add(sop);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSop(Sop sop)
        {
            if (db.Entry(sop).State == EntityState.Detached)
            {
                db.Sops.Attach(sop);
            }
            db.Sops.Remove(sop);
            await db.SaveChangesAsync();
        }

        public IEnumerable<Sop> GetSopsByCategoryId(int categoryId)
        {
            return db.Sops.Where(m => m.Category_Id == categoryId).ToList();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Sop> GetSopById(int id)
        {
            return await db.Sops.FindAsync(id);
        }

        public IEnumerable<Sop> GetSops()
        {
            return db.Sops.ToList();
        }

        public async Task UpdateSop(Sop sop)
        {
            db.Entry(sop).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public bool DoesSopFileNameExist(string fileName)
        {
            return db.Sops.Where(m => m.FileName == fileName).Count() != 0;
        }
    }
}
