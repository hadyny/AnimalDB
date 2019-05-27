using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SurgicalNoteService : ISurgicalNoteService
    {
        private readonly IRepository<SurgicalNote> _surgicalNotes;

        public SurgicalNoteService(IRepository<SurgicalNote> surgicalNotes)
        {
            this._surgicalNotes = surgicalNotes;
        }

        public async Task CreateSurgicalNote(SurgicalNote SurgicalNote)
        {
            _surgicalNotes.Insert(SurgicalNote);
            await _surgicalNotes.Save();
        }

        public async Task DeleteSurgicalNote(SurgicalNote SurgicalNote)
        {
            await _surgicalNotes.Delete(SurgicalNote.Id);
            await _surgicalNotes.Save();
        }
        public async Task<SurgicalNote> GetSurgicalNoteById(int id)
        {
            return await _surgicalNotes.GetById(id);
        }

        public async Task<IEnumerable<SurgicalNote>> GetSurgicalNotes()
        {
            return await _surgicalNotes.GetAll();
        }

        public async Task UpdateSurgicalNote(SurgicalNote surgicalNote)
        {
            _surgicalNotes.Update(surgicalNote);
            await _surgicalNotes.Save();
        }

        public async Task<IEnumerable<SurgicalNote>> GetSurgicalNotesByAnimalId(int animalId)
        {
            return await _surgicalNotes.GetAll(m => m.Animal_Id == animalId);
        }
    }
}
