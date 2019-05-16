using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animal>> GetAllAnimals();

        Task<IEnumerable<Animal>> GetLivingAnimals();

        Task<EthicsNumberHistory> GetAnimalsEthicsNumber(int animalId);

        Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId);

        Task<DateTime?> GetAnimalsGDDate(int animalId);

        Task<string> GetAnimalsGDDescription(int animalId);

        Task<DateTime?> GetAnimalsPlugDate(int animalId);

        Task<IEnumerable<Animal>> GetAnimalsLivingOffspring(int animalId);

        Task<string> GetAnimalsEthicsNumberDescription(int animalId);

        Task<CageLocationHistory> GetAnimalsCageLocation(int animalId);

        Task<string> GetAnimalsCageLocationDescription(int animalId);

        Task<DateTime?> GetAnimalsSurgeryDate(int animalId);

        Task AddPhotoToAnimal(int animalId);

        Task RemovePhotoFromAnimal(int animalId);

        Task<string> GetAnimalsListOfSurgeries(int animalId);

        Task<string> GetAnimalsSurgeon(int animalId);

        Task<string> GetAnimalsInjectionSurgeon(int animalId);

        Task<DateTime?> GetAnimalsInjectionDate(int animalId);

        Task<IEnumerable<Animal>> GetLivingAnimalsNotCheckedToday();

        Task<IEnumerable<Animal>> GetLivingAnimalsNotCheckedTodayForEmailing();

        Task<IEnumerable<Animal>> GetLivingAnimalsNotFedTodayForEmailing();

        Task<IEnumerable<Animal>> GetDeceasedAnimals();

        Task<IEnumerable<Animal>> GetInvestigatorsAnimals(string userName);

        Task<IEnumerable<Animal>> GetStudentsAnimals(string userName);

        Task CreateAnimal(Animal animal);

        Task<Animal> GetAnimalById(int id);

        Task<Animal> GetAnimalByUniqueId(string id);

        Task UpdateAnimal(Animal animal);

        Task DeleteAnimal(Animal animal);

        Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId);

        Task UpdateEthicsNumberHistoryForAnimalId(EthicsNumberHistory ethicsnumberhistory);

        Task<int?> GetAnimalsCurrentWeight(int animalId);

        Task<Animal> GetAnimalByTag(string animalId);

        Task Resurrect(int id);

        Task ReturnAnimalToStock(int id);

        Task BulkUpdateAnimals(List<Animal> animals, Grading? grading, Manipulation? manipulation, int? ArrivalStatus_Id);
    }
}