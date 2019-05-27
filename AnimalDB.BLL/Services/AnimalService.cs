using System.Collections.Generic;
using System.Linq;
using AnimalDB.Repo.Interfaces;
using System.Threading.Tasks;
using System.Data.Entity;
using AnimalDB.Repo.Entities;
using System;
using System.Text;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Repositories.Abstract;

namespace AnimalDB.Repo.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IRepository<Animal> _animals;
        private readonly IRepository<EthicsNumberHistory> _ethicsNumberHistories;
        private readonly IRepository<EthicsNumber> _ethicsNumbers;
        private readonly IRepository<CageLocationHistory> _cageLocationHistories;
        private readonly IRepository<Student> _students;

        public AnimalService(
            IRepository<Animal> animals,
            IRepository<EthicsNumberHistory> ethicsNumberHistories,
            IRepository<EthicsNumber> ethicsNumbers,
            IRepository<CageLocationHistory> cageLocationHistories,
            IRepository<Student> students
            )
        {
            this._animals = animals;
            this._ethicsNumberHistories = ethicsNumberHistories;
            this._ethicsNumbers = ethicsNumbers;
            this._cageLocationHistories = cageLocationHistories;
            this._students = students;
        }

        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            var animals = await _animals.GetAll();
            return animals
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId);
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimals()
        {
            var animals = await this._animals.GetAll(m => !m.DeathDate.HasValue);
            return animals
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId);
        }

        public async Task<EthicsNumberHistory> GetAnimalsEthicsNumber(int animalId)
        {
            var _histories = await _ethicsNumberHistories.GetAll(m => m.Animal_Id == animalId);

            return _histories
                        .OrderByDescending(m => m.Timestamp)
                        .FirstOrDefault();
        }

        public async Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId)
        {
            var animal = await _animals.GetById(animalId);

            return animal?.SurgicalNotes.Count(m => m.SurgeryType.Description == "Plug") == 0 ? (SurgicalNote)null :
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Plug");
        }

        public async Task<Animal> GetAnimalByTag(string animalId)
        {
            var _animals = await this._animals.GetAll(m => m.Tag == animalId);
            return _animals.FirstOrDefault();
        }

        public async Task<DateTime?> GetAnimalsGDDate(int animalId)
        {
            var surgery = await GetAnimalsPlugSurgery(animalId);
            var plugDate = await GetAnimalsPlugDate(animalId);
            if (!plugDate.HasValue || surgery == null) return null;
            int days = surgery.GDTimeline == null ? 0 : surgery.GDTimeline.Offset;

            return plugDate.HasValue ? plugDate.Value.AddDays(days) : (DateTime?) null;
        }

        public async Task<string> GetAnimalsGDDescription(int animalId)
        {
            var surgery = await GetAnimalsPlugSurgery(animalId);
            return surgery?.GDTimeline?.Description;
        }

        public async Task<DateTime?> GetAnimalsPlugDate(int animalId)
        {
            var surgery = await GetAnimalsPlugSurgery(animalId);
            return surgery?.Timestamp;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsLivingOffspring(int animalId)
        {
            var _animal = await _animals.GetById(animalId);
            return _animal?.Offspring.Where(m => !m.DeathDate.HasValue);
        }

        public async Task<string> GetAnimalsEthicsNumberDescription(int animalId)
        {
            EthicsNumberHistory ethics = await GetAnimalsEthicsNumber(animalId);
            return ethics?.EthicsNumber.Text;
        }

        public async Task<CageLocationHistory> GetAnimalsCageLocation(int animalId)
        {
            var _histories = await _cageLocationHistories.GetAll(m => m.Animal_Id == animalId);
            return _histories
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();
        }

        public async Task<DateTime?> GetAnimalsSurgeryDate(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            return animal?.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? (DateTime?)null : 
                    animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Timestamp;
        }

        public async Task AddPhotoToAnimal(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            animal.HasPicture = true;
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task RemovePhotoFromAnimal(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            animal.HasPicture = false;
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task<string> GetAnimalsListOfSurgeries(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            
            StringBuilder list = new StringBuilder();

            foreach (var note in animal.SurgicalNotes.Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug"))
            {
                list.Append(note.SurgeryType.Description);
                list.Append(note.VirusType_Id != null ? "(" + note.VirusType.Description + ")" : "");
                list.Append(", ");
            }

            if (list.Length != 0)
            {
                list.Remove(list.Length - 1, 1);
            }

            return list.ToString();
        }

        public async Task UpdateEthicsNumberHistoryForAnimalId(EthicsNumberHistory ethicsnumberhistory)
        {
            var ethics = await _ethicsNumbers.GetById(ethicsnumberhistory.Ethics_Id);
            var animal = await _animals.GetById(ethicsnumberhistory.Animal_Id);
            animal.Investigator_Id = ethics.Investigator_Id;
            _ethicsNumberHistories.Update(ethicsnumberhistory);
            await _ethicsNumberHistories.Save();
        }

        public async Task<string> GetAnimalsSurgeon(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Surgeon;
        }

        public async Task<string> GetAnimalsInjectionSurgeon(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description == "Injection") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Injection").Surgeon;
        }

        public async Task<int?> GetAnimalsCurrentWeight(int animalId)
        {
            var _animal = await _animals.GetById(animalId);
            return _animal
                        .Feeds
                        .Where(m => m.Weight != null)
                        .OrderByDescending(m => m.Date)
                        .FirstOrDefault()?
                        .Weight;
        }

        public async Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId)
        {
            var animal = await _animals.GetById(animalId);
            var currentEthics = await GetAnimalsEthicsNumber(animalId);
            if (currentEthics != null && currentEthics.Ethics_Id == ethicsNumberId)
            {
                return;
            }

            var ethicsNumber = await _ethicsNumbers.GetById(ethicsNumberId);
            _ethicsNumberHistories.Insert(new EthicsNumberHistory() { Animal_Id = animalId, Ethics_Id = ethicsNumberId, Timestamp = DateTime.Now });

            animal.Investigator_Id = ethicsNumber.Investigator_Id;
            animal.StockAnimal = false;
            _animals.Update(animal);

            await _ethicsNumberHistories.Save();
            await _animals.Save();
        }

        public async Task<DateTime?> GetAnimalsInjectionDate(int animalId)
        {
            var animal = await _animals.GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description == "Injection") == 0 ? (DateTime?)null : 
                        animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Injection").Timestamp;
        }

        public async Task<string> GetAnimalsCageLocationDescription(int animalId)
        {
            CageLocationHistory location = await GetAnimalsCageLocation(animalId);
            if (location == null) return "";
            return location.CageLocation_Id != null ?
                                    location.CageLocation.Description :
                                    location.RackEntry.Rack.Reference_Id + ", " + location.RackEntry.Reference;
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimalsNotCheckedToday()
        {
            var animals = await _animals.GetAll(m => m.DeathDate == null && DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now));
            return animals
                        .OrderBy(m => m.UniqueAnimalId.Length);
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimalsNotCheckedTodayForEmailing()
        {
            return await _animals.GetAll(m => m.DeathDate == null && DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now) && m.Room.EmailUpdates);
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimalsNotFedTodayForEmailing()
        {
            return await _animals.GetAll(m => m.DeathDate == null && (m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault() == null ||
                                    DbFunctions.TruncateTime(m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault().Date) != DbFunctions.TruncateTime(DateTime.Now)) &&
                                    m.Room.EmailUpdates);
        }

        public async Task<IEnumerable<Animal>> GetDeceasedAnimals()
        {
            var animals = await this._animals.GetAll(m => m.DeathDate.HasValue);
            return animals
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId);
        }

        public async Task<IEnumerable<Animal>> GetInvestigatorsAnimals(string userName)
        {
            var animals = await GetLivingAnimals();
            return animals.Where(m => m.Investigator.UserName == userName);
        }

        public async Task<IEnumerable<Animal>> GetStudentsAnimals(string userName)
        {
            var students = await this._students.GetAll();
            var animals = await GetLivingAnimals();
            var student = students.Single(m => m.UserName == userName);
            return (student == null) ? null : animals.Where(m => student.Investigators.Contains(m.Investigator));
        }

        public async Task CreateAnimal(Animal animal)
        {
            _animals.Insert(animal);
            await _animals.Save();
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            return await _animals.GetById(id);
        }

        public async Task<Animal> GetAnimalByUniqueId(string id)
        {
            var animals = await this._animals.GetAll(m => m.UniqueAnimalId.ToUpper() == id.ToUpper());
            return animals.SingleOrDefault();
        }

        public async Task UpdateAnimal(Animal animal)
        {
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task DeleteAnimal(Animal animal)
        {
            animal.Parents.Clear();
            animal.Offspring.Clear();
            _animals.Update(animal);
            await _animals.Delete(animal.Id);
            await _animals.Save();
        }

        public async Task ReturnAnimalToStock(int id)
        {
            var animal = await _animals.GetById(id);

            animal.EthicsNumbers.Clear();
            foreach (var ethics in await _ethicsNumberHistories.GetAll(m => m.Animal_Id == id))
            {
                await _ethicsNumberHistories.Delete(ethics.Id);
            }
            animal.StockAnimal = true;

            await _ethicsNumberHistories.Save();
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task Resurrect(int id)
        {
            var animal = await _animals.GetById(id);
            animal.DeathDate = null;
            animal.CauseOfDeath = null;
            _animals.Update(animal);
            await _animals.Save();
        }

        public async Task BulkUpdateAnimals(List<Animal> animals, Grading? grading, Manipulation? manipulation, int? ArrivalStatus_Id)
        {
            if (grading != null)
            {
                foreach (var animal in animals)
                {
                    animal.Grading = grading.Value;
                }
            }

            if (manipulation != null)
            {
                foreach (var animal in animals)
                {
                    animal.Manipulation = manipulation.Value;
                }
            }

            if (ArrivalStatus_Id != null)
            {
                foreach (var animal in animals)
                {
                    animal.ArrivalStatus_Id = ArrivalStatus_Id;
                }
            }

            await this._animals.Save();
        }
    }
}