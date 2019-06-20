using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        IEnumerable<Animal> GetLiving();
        EthicsNumberHistory GetAnimalsEthicsNumber(int animalId);
        Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId);
        Animal GetAnimalByTag(string animalId);
        Task<DateTime?> GetAnimalsGDDate(int animalId);
        Task<string> GetAnimalsGDDescription(int animalId);
        Task<DateTime?> GetAnimalsPlugDate(int animalId);
        Task<IEnumerable<Animal>> GetAnimalsLivingOffspring(int animalId);
        string GetAnimalsEthicsNumberDescription(int animalId);
        CageLocationHistory GetAnimalsCageLocation(int animalId);
        Task<DateTime?> GetAnimalsSurgeryDate(int animalId);
        Task AddPhotoToAnimal(int animalId);
        Task RemovePhotoFromAnimal(int animalId);
        Task<string> GetAnimalsListOfSurgeries(int animalId);
        Task UpdateEthicsNumberHistoryForAnimalId(EthicsNumberHistory ethicsnumberhistory);
        Task<string> GetAnimalsSurgeon(int animalId);
        Task<string> GetAnimalsInjectionSurgeon(int animalId);
        Task<int?> GetAnimalsCurrentWeight(int animalId);
        Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId);
        Task<DateTime?> GetAnimalsInjectionDate(int animalId);
        string GetAnimalsCageLocationDescription(int animalId);
        IEnumerable<Animal> GetLivingAnimalsNotCheckedToday();
        IEnumerable<Animal> GetLivingAnimalsNotCheckedTodayForEmailing();
        IEnumerable<Animal> GetLivingAnimalsNotFedTodayForEmailing();
        IEnumerable<Animal> GetDeceasedAnimals();
        IEnumerable<Animal> GetInvestigatorsAnimals(string userName);
        IEnumerable<Animal> GetStudentsAnimals(string userName);
        Task<Animal> GetAnimalByUniqueId(string id);
        Task ReturnAnimalToStock(int id);
        Task Resurrect(int id);
        void BulkUpdateAnimals(List<Animal> animals, Grading? grading, Manipulation? manipulation, int? ArrivalStatus_Id);
    }
}
