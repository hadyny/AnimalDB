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
    public class RosterNoteRepo : IRosterNote, IDisposable
    {
        private AnimalDBContext db;

        public RosterNoteRepo()
        {
            this.db = new AnimalDBContext();
        }

        public RosterNoteRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateRosterNote(RosterNote rosterNote)
        {
            db.RosterNotes.Add(rosterNote);
            await db.SaveChangesAsync();
        }

        public async Task DeleteRosterNote(RosterNote rosterNote)
        {
            if (db.Entry(rosterNote).State == EntityState.Detached)
            {
                db.RosterNotes.Attach(rosterNote);
            }
            db.RosterNotes.Remove(rosterNote);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public Task<RosterNote> GetRosterNoteById(int id)
        {
            return db.RosterNotes.FindAsync(id);
        }

        public IEnumerable<RosterNote> GetRosterNotes()
        {
            return db.RosterNotes.ToList();
        }

        public IEnumerable<RosterNote> GetRosterNotesByRosterId(int id)
        {
            return db.RosterNotes.Where(m => m.Roster_Id == id).ToList();
        }

        public async Task UpdateRosterNote(RosterNote rosterNote)
        {
            db.Entry(rosterNote).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
