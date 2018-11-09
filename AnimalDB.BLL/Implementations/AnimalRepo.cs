using System.Collections.Generic;
using System.Linq;
using AnimalDB.Repo.Interfaces;
using System.Threading.Tasks;
using System.Data.Entity;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Contexts;
using System;
using System.Text;

namespace AnimalDB.Repo.Implementations
{
    public class AnimalRepo : IAnimal, IDisposable
    {
        private IAnimalDBContext db;

        public AnimalRepo() => this.db = new AnimalDBContext();

        public AnimalRepo(IAnimalDBContext db) => this.db = db;

        public IEnumerable<Animal> GetAllAnimals()
        {
            return db.Animals
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public IEnumerable<Animal> GetLivingAnimals()
        {
            return db.Animals
                        .Where(m => !m.DeathDate.HasValue)
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public async Task<EthicsNumberHistory> GetAnimalsEthicsNumber(int animalId)
        {
            return await db.EthicsNumberHistories
                        .Where(m => m.Animal_Id == animalId)
                        .OrderByDescending(m => m.Timestamp)
                        .FirstOrDefaultAsync();
        }

        public async Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId)
        {
            var animal = await db.Animals.FindAsync(animalId);

            return animal?.SurgicalNotes.Count(m => m.SurgeryType.Description == "Plug") == 0 ? (SurgicalNote)null :
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Plug");
        }

        public async Task<Animal> GetAnimalByTag(string animalId)
        {
            return await db.Animals.FirstOrDefaultAsync(m => m.Tag == animalId);
        }

        public DateTime? GetAnimalsGDDate(int animalId)
        {
            var surgery = GetAnimalsPlugSurgery(animalId).Result;
            var plugDate = GetAnimalsPlugDate(animalId);
            if (!plugDate.HasValue || surgery == null) return null;
            int days = surgery.GDTimeline == null ? 0 : surgery.GDTimeline.Offset;

            return plugDate.HasValue ? plugDate.Value.AddDays(days) : (DateTime?) null;
        }

        public async Task<string> GetAnimalsGDDescription(int animalId)
        {
            var surgery = await GetAnimalsPlugSurgery(animalId);
            return surgery?.GDTimeline?.Description;
        }

        public DateTime? GetAnimalsPlugDate(int animalId)
        {
            var surgery = GetAnimalsPlugSurgery(animalId);
            return surgery.Result?.Timestamp;
        }

        public IEnumerable<Animal> GetAnimalsLivingOffspring(int animalId)
        {
            return db.Animals.Find(animalId)?.Offspring.Where(m => !m.DeathDate.HasValue);
        }

        public async Task<string> GetAnimalsEthicsNumberDescription(int animalId)
        {
            EthicsNumberHistory ethics = await GetAnimalsEthicsNumber(animalId);
            return ethics?.EthicsNumber.Text;
        }

        public async Task<CageLocationHistory> GetAnimalsCageLocation(int animalId)
        {
            return await db.CageLocationHistories
                .Include(m => m.CageLocation)
                .Where(m => m.Animal_Id == animalId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }

        public DateTime? GetAnimalsSurgeryDate(int animalId)
        {
            var animal = db.Animals.FindAsync(animalId).Result;
            return animal?.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? (DateTime?)null : 
                    animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Timestamp;
        }

        public void AddPhotoToAnimal(int animalId)
        {
            var animal = db.Animals.Find(animalId);
            animal.HasPicture = true;
            db.SaveChanges();
        }

        public void RemovePhotoFromAnimal(int animalId)
        {
            var animal = db.Animals.Find(animalId);
            animal.HasPicture = false;
            db.SaveChanges();
        }

        public async Task<string> GetAnimalsListOfSurgeries(int animalId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            
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
            var ethics = db.EthicsNumbers.Find(ethicsnumberhistory.Ethics_Id);
            var animal = db.Animals.Find(ethicsnumberhistory.Animal_Id);
            animal.Investigator_Id = ethics.Investigator_Id;
            db.Entry(ethicsnumberhistory).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<string> GetAnimalsSurgeon(int animalId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug").Surgeon;
        }

        public async Task<string> GetAnimalsInjectionSurgeon(int animalId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            if (animal == null) throw new NullReferenceException();
            return animal.SurgicalNotes.Count(m => m.SurgeryType.Description == "Injection") == 0 ? "" : 
                animal.SurgicalNotes.OrderBy(m => m.Timestamp).Last(m => m.SurgeryType.Description == "Injection").Surgeon;
        }

        public int? GetAnimalsCurrentWeight(int animalId)
        {
            return db.Animals
                        .Find(animalId)
                        .Feeds
                        .Where(m => m.Weight != null)
                        .OrderByDescending(m => m.Date)
                        .FirstOrDefault()?
                        .Weight;
        }

        public async Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId)
        {
            var animal = await db.Animals.FindAsync(animalId);
            var currentEthics = await GetAnimalsEthicsNumber(animalId);
            if (currentEthics != null && currentEthics.Ethics_Id == ethicsNumberId)
            {
                return;
            }

            var investigatorId = db.EthicsNumbers.Find(ethicsNumberId).Investigator_Id;
            var ethicsNumber = new EthicsNumberHistory() { Animal_Id = animalId, Ethics_Id = ethicsNumberId, Timestamp = DateTime.Now };
            animal.EthicsNumbers.Add(ethicsNumber);
            animal.Investigator_Id = investigatorId;
            animal.StockAnimal = false;
            await db.SaveChangesAsync();
        }

        public DateTime? GetAnimalsInjectionDate(int animalId)
        {
            var animal = db.Animals.FindAsync(animalId).Result;
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

        public IEnumerable<Animal> GetLivingAnimalsNotCheckedToday()
        {
            return db.Animals
                        .Where(m => !m.DeathDate.HasValue && DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now))
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public IEnumerable<Animal> GetLivingAnimalsNotCheckedTodayForEmailing()
        {
            return db.Animals
                        .Where(m => 
                                !m.DeathDate.HasValue && 
                                DbFunctions.TruncateTime(m.LastChecked) != DbFunctions.TruncateTime(DateTime.Now) && 
                                m.Room.EmailUpdates)
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();

        }

        public IEnumerable<Animal> GetLivingAnimalsNotFedTodayForEmailing()
        {
            return db.Animals
                        .Where(m => !m.DeathDate.HasValue && 
                                    (m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault() == null ||
                                    DbFunctions.TruncateTime(m.Feeds.OrderByDescending(n => n.Date).FirstOrDefault().Date) != DbFunctions.TruncateTime(DateTime.Now)) &&
                                    m.Room.EmailUpdates)
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public IEnumerable<Animal> GetDeceasedAnimals()
        {
            return db.Animals
                        .Where(m => m.DeathDate.HasValue)
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public IEnumerable<Animal> GetInvestigatorsAnimals(string userName)
        {
            return db.Animals
                        .Where(m => !m.DeathDate.HasValue && m.Investigator.UserName == userName)
                        .OrderBy(m => m.UniqueAnimalId.Length)
                        .ThenBy(m => m.UniqueAnimalId)
                        .ToList();
        }

        public IEnumerable<Animal> GetStudentsAnimals(string userName)
        {
            var student = db.Students.Single(m => m.UserName == userName);
            return (student == null) ? null : db.Animals
                                                    .Where(m => !m.DeathDate.HasValue)
                                                    .OrderBy(m => m.UniqueAnimalId.Length)
                                                    .ThenBy(m => m.UniqueAnimalId)
                                                    .ToList()
                                                    .Where(m => student.Investigators.Contains(m.Investigator));
        }

        public async Task CreateAnimal(Animal animal)
        {
            db.Animals.Add(animal);
            await db.SaveChangesAsync();
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            return await db.Animals.FindAsync(id);
        }

        public Animal GetAnimalByUniqueId(string id)
        {
            return db.Animals.AsNoTracking().SingleOrDefault(m => m.UniqueAnimalId.ToUpper() == id.ToUpper());
        }

        public async Task UpdateAnimal(Animal animal)
        {
            db.Entry(animal).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task DeleteAnimal(Animal animal)
        {
            if (db.Entry(animal).State == EntityState.Detached)
            {
                db.Animals.Attach(animal);
            }
            animal.Parents.Clear();
            animal.Offspring.Clear();
            db.Animals.Remove(animal);
            await db.SaveChangesAsync();
        }

        public async Task ReturnAnimalToStock(int id)
        {
            var animal = await db.Animals.FindAsync(id);

            animal.EthicsNumbers.Clear();
            foreach (var ethics in db.EthicsNumberHistories.Where(m => m.Animal_Id == id))
            {
                db.EthicsNumberHistories.Remove(ethics);
            }
            animal.StockAnimal = true;

            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task Resurrect(int id)
        {
            var animal = await db.Animals.FindAsync(id);
            animal.DeathDate = null;
            animal.CauseOfDeath = null;
            await db.SaveChangesAsync();
        }
    }
}