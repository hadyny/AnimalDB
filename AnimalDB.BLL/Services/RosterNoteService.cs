using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class RosterNoteService : IRosterNoteService
    {
        private readonly IRepository<RosterNote> _rosterNotes;

        public RosterNoteService(IRepository<RosterNote> rosterNotes)
        {
            this._rosterNotes = rosterNotes;
        }

        public async Task CreateRosterNote(RosterNote rosterNote)
        {
            _rosterNotes.Insert(rosterNote);
            await _rosterNotes.Save();
        }

        public async Task DeleteRosterNote(RosterNote rosterNote)
        { 
            await _rosterNotes.Delete(rosterNote.Id);
            await _rosterNotes.Save();
        }

        public async Task<RosterNote> GetRosterNoteById(int id)
        {
            return await _rosterNotes.GetById(id);
        }

        public async Task<IEnumerable<RosterNote>> GetRosterNotes()
        {
            return await _rosterNotes.GetAll();
        }

        public async Task<IEnumerable<RosterNote>> GetRosterNotesByRosterId(int id)
        {
            return await _rosterNotes.GetAll(m => m.Roster_Id == id);
        }

        public async Task UpdateRosterNote(RosterNote rosterNote)
        {
            _rosterNotes.Update(rosterNote);
            await _rosterNotes.Save();
        }
    }
}
