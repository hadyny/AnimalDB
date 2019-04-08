using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface INote
    {
        IEnumerable<Note> GetNotes();

        Task CreateNote(Note note);

        Task<Note> GetNoteById(int id);

        Task UpdateNote(Note note);

        Task DeleteNote(Note note);

        IEnumerable<Note> GetNoteByAnimalId(int animalId);
    }
}
