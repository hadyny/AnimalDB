using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISurgicalNote
    {
        IEnumerable<SurgicalNote> GetSurgicalNotes();

        Task CreateSurgicalNote(SurgicalNote surgicalNote);

        Task<SurgicalNote> GetSurgicalNoteById(int id);

        Task UpdateSurgicalNote(SurgicalNote surgicalNote);

        Task DeleteSurgicalNote(SurgicalNote surgicalNote);

        IEnumerable<SurgicalNote> GetSurgicalNoteByAnimalId(int animalId);
    }
}
