using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        public AnimalRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<Animal> GetLiving()
        {
            return Context
                    .Animals
                    .Where(m => !m.DeathDate.HasValue)
                    .OrderBy(m => m.UniqueAnimalId.Length)
                    .ThenBy(m => m.UniqueAnimalId)
                    .ToList();
        }

        public EthicsNumberHistory GetAnimalsEthicsNumber(int animalId)
        {
            return Context
                    .EthicsNumberHistories
                    .Where(m => m.Animal_Id == animalId)
                    .OrderByDescending(m => m.Timestamp)
                    .FirstOrDefault();
        }

        public async Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId)
        {
            var animal = await GetById(animalId);

            return animal?
                        .SurgicalNotes
                        .Count(m =>
                                m.SurgeryType.Description == "Plug") == 0 ? 
                        (SurgicalNote)null :
                    animal
                        .SurgicalNotes
                        .OrderBy(m => 
                                    m.Timestamp)
                        .Last(m => m.SurgeryType.Description == "Plug");
        }

        public Animal GetAnimalByTag(string animalId)
        {
            return Context
                    .Animals
                    .Where(m => m.Tag == animalId)
                    .FirstOrDefault();
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
            var _animal = await GetById(animalId);
            return _animal?.Offspring.Where(m => !m.DeathDate.HasValue).ToList();
        }

        public string GetAnimalsEthicsNumberDescription(int animalId)
        {
            EthicsNumberHistory ethics = GetAnimalsEthicsNumber(animalId);
            return ethics?.EthicsNumber.Text;
        }

        public CageLocationHistory GetAnimalsCageLocation(int animalId)
        {
            var _histories = Context.CageLocationHistories.Where(m => m.Animal_Id == animalId);
            return _histories
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefault();
        }

        public async Task<DateTime?> GetAnimalsSurgeryDate(int animalId)
        {
            var animal = await GetById(animalId);
            return animal?.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? (DateTime?)null : 
                    animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Timestamp;
        }

        public async Task AddPhotoToAnimal(int animalId)
        {
            var animal = await GetById(animalId);
            animal.HasPicture = true;
            Context.Entry(animal).State = EntityState.Modified;
        }

        public async Task RemovePhotoFromAnimal(int animalId)
        {
            var animal = await GetById(animalId);
            animal.HasPicture = false;
            Context.Entry(animal).State = EntityState.Modified;
        }

        public async Task<string> GetAnimalsListOfSurgeries(int animalId)
        {
            var animal = await GetById(animalId);
            
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
            var ethics = await Context.EthicsNumbers.FindAsync(ethicsnumberhistory.Ethics_Id);
            var animal = await GetById(ethicsnumberhistory.Animal_Id);
            animal.Investigator_Id = ethics.Investigator_Id;
            Context.Entry(ethicsnumberhistory).State = EntityState.Modified;
        }

        public async Task<string> GetAnimalsSurgeon(int animalId)
        {
            var animal = await GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Surgeon;
        }

        public async Task<string> GetAnimalsInjectionSurgeon(int animalId)
        {
            var animal = await GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description == "Injection") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Injection").Surgeon;
        }

        public async Task<int?> GetAnimalsCurrentWeight(int animalId)
        {
            var _animal = await GetById(animalId);
            return _animal
                        .Feeds
                        .Where(m => m.Weight != null)
                        .OrderByDescending(m => m.Date)
                        .FirstOrDefault()?
                        .Weight;
        }

        public async Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId)
        {
            var animal = await GetById(animalId);
            var currentEthics = GetAnimalsEthicsNumber(animalId);
            if (currentEthics != null && currentEthics.Ethics_Id == ethicsNumberId)
            {
                return;
            }

            var ethicsNumber = await Context.EthicsNumbers.FindAsync(ethicsNumberId);
            Context.EthicsNumberHistories.Add(new EthicsNumberHistory() { Animal_Id = animalId, Ethics_Id = ethicsNumberId, Timestamp = DateTime.Now });

            animal.Investigator_Id = ethicsNumber.Investigator_Id;
            animal.StockAnimal = false;
            Context.Entry(animal).State = EntityState.Modified;
        }

        public async Task<DateTime?> GetAnimalsInjectionDate(int animalId)
        {
            var animal = await GetById(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description == "Injection") == 0 ? (DateTime?)null : 
                        animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Injection").Timestamp;
        }

        public string GetAnimalsCageLocationDescription(int animalId)
        {
            CageLocationHistory location = GetAnimalsCageLocation(animalId);
            if (location == null) return "";
            return location.CageLocation_Id != null ?
                                    location.CageLocation.Description :
                                    location.RackEntry.Rack.Reference_Id + ", " + location.RackEntry.Reference;
        }

        public IEnumerable<Animal> GetLivingAnimalsNotCheckedToday()
        {
            return Context
                    .Animals
                    .Where(m => 
                            m.DeathDate == null && 
                            DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now))
                    .OrderBy(m => m.UniqueAnimalId.Length)
                    .ToList();
        }

        public IEnumerable<Animal> GetLivingAnimalsNotCheckedTodayForEmailing()
        {
            return Context
                    .Animals
                    .Where(m => 
                            m.DeathDate == null && 
                            DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now) 
                            && m.Room.EmailUpdates)
                    .ToList();
        }

        public IEnumerable<Animal> GetLivingAnimalsNotFedTodayForEmailing()
        {
            return Context
                    .Animals
                    .Where(m => 
                            m.DeathDate == null && 
                            (m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault() == null ||
                                DbFunctions.TruncateTime(m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault().Date) != DbFunctions.TruncateTime(DateTime.Now)) &&
                            m.Room.EmailUpdates)
                    .ToList();
        }

        public IEnumerable<Animal> GetDeceasedAnimals()
        {
            return Context
                    .Animals
                    .Where(m => m.DeathDate.HasValue)
                    .OrderBy(m => m.UniqueAnimalId.Length)
                    .ThenBy(m => m.UniqueAnimalId)
                    .ToList();
        }

        public IEnumerable<Animal> GetInvestigatorsAnimals(string userName)
        {
            return GetLiving()
                    .Where(m => m.Investigator.UserName == userName)
                    .ToList();
        }

        public IEnumerable<Animal> GetStudentsAnimals(string userName)
        {
            var animals = GetLiving();
            var student = Context.Students.Single(m => m.UserName == userName);
            var investigators = student.Investigators;
            return (student == null) ? null : animals.Where(m => investigators.Any(n => n.Id == m.Investigator_Id)).ToList();
        }

        public async Task<Animal> GetAnimalByUniqueId(string id)
        {
            return await Context
                            .Animals
                            .SingleOrDefaultAsync(m => m.UniqueAnimalId.ToUpper() == id.ToUpper());
        }

        public async Task ReturnAnimalToStock(int id)
        {
            var animal = await GetById(id);

            animal.EthicsNumbers.Clear();
            foreach (var ethics in Context.EthicsNumberHistories.Where(m => m.Animal_Id == id))
            {
                Context.EthicsNumberHistories.Remove(ethics);
            }
            animal.StockAnimal = true;

            Context.Entry(animal).State = EntityState.Modified;
        }

        public async Task Resurrect(int id)
        {
            var animal = await GetById(id);
            animal.DeathDate = null;
            animal.CauseOfDeath = null;
            Context.Entry(animal).State = EntityState.Modified;
        }

        public void BulkUpdateAnimals(List<Animal> animals, Grading? grading, Manipulation? manipulation, int? ArrivalStatus_Id)
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
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}