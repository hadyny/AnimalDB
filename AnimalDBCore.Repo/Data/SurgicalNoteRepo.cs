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
    public class SurgicalNoteRepo : ISurgicalNote, IDisposable
    {
        private AnimalDBContext db;

        public SurgicalNoteRepo()
        {
            this.db = new AnimalDBContext();
        }
        public SurgicalNoteRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSurgicalNote(SurgicalNote SurgicalNote)
        {
            db.SurgicalNotes.Add(SurgicalNote);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSurgicalNote(SurgicalNote SurgicalNote)
        {
            if (db.Entry(SurgicalNote).State == EntityState.Detached)
            {
                db.SurgicalNotes.Attach(SurgicalNote);
            }
            db.SurgicalNotes.Remove(SurgicalNote);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<SurgicalNote> GetSurgicalNoteById(int id)
        {
            return await db.SurgicalNotes.FindAsync(id);
        }

        public IEnumerable<SurgicalNote> GetSurgicalNotes()
        {
            return db.SurgicalNotes.ToList();
        }

        public async Task UpdateSurgicalNote(SurgicalNote SurgicalNote)
        {
            db.Entry(SurgicalNote).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<SurgicalNote> GetSurgicalNoteByAnimalId(int animalId)
        {
            return db.SurgicalNotes.Where(m => m.Animal_Id == animalId).ToList();
        }
    }
}
