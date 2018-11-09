using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRosterNote
    {
        IEnumerable<RosterNote> GetRosterNotes();

        Task CreateRosterNote(RosterNote rosterNote);

        Task<RosterNote> GetRosterNoteById(int id);

        IEnumerable<RosterNote> GetRosterNotesByRosterId(int id);

        Task UpdateRosterNote(RosterNote rosterNote);

        Task DeleteRosterNote(RosterNote rosterNote);
    }
}
