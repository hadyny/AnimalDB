using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class NoteRepo : INote, IDisposable
    {
        private AnimalDBContext db;

        public NoteRepo()
        {
            this.db = new AnimalDBContext();
        }
        public NoteRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateNote(Note note)
        {
            db.Notes.Add(note);
            await db.SaveChangesAsync();
        }

        public async Task DeleteNote(Note note)
        {
            if (db.Entry(note).State == EntityState.Detached)
            {
                db.Notes.Attach(note);
            }
            db.Notes.Remove(note);
            await db.SaveChangesAsync();
        }

        public IEnumerable<Note> GetNoteByAnimalId(int animalId)
        {
            return db.Notes.Where(m => m.Animal_Id == animalId).ToList();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Note> GetNoteById(int id)
        {
            return await db.Notes.FindAsync(id);
        }

        public IEnumerable<Note> GetNotes()
        {
            return db.Notes.ToList();
        }

        public async Task UpdateNote(Note note)
        {
            db.Entry(note).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
