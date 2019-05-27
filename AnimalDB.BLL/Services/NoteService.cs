using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _notes;

        public NoteService(IRepository<Note> notes)
        {
            this._notes = notes;
        }

        public async Task CreateNote(Note note)
        {
            _notes.Insert(note);
            await _notes.Save();
        }

        public async Task DeleteNote(Note note)
        {
            await _notes.Delete(note.Id);
            await _notes.Save();
        }

        public async Task<IEnumerable<Note>> GetNotesByAnimalId(int animalId)
        {
            return await _notes.GetAll(m => m.Animal_Id == animalId);
        }

        public async Task<Note> GetNoteById(int id)
        {
            return await _notes.GetById(id);
        }

        public async Task<IEnumerable<Note>> GetNotes()
        {
            return await _notes.GetAll();
        }

        public async Task UpdateNote(Note note)
        {
            _notes.Update(note);
            await _notes.Save();
        }
    }
}
