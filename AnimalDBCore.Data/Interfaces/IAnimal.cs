using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  AnimalDBCore.Core.Interfaces
{
    public interface IAnimal
    {
        IEnumerable<Animal> GetAllAnimals();

        IEnumerable<Animal> GetLivingAnimals();

        Task<EthicsNumberHistory> GetAnimalsEthicsNumber(int animalId);

        Task<SurgicalNote> GetAnimalsPlugSurgery(int animalId);

        DateTime? GetAnimalsGDDate(int animalId);

        Task<string> GetAnimalsGDDescription(int animalId);

        DateTime? GetAnimalsPlugDate(int animalId);

        IEnumerable<Animal> GetAnimalsLivingOffspring(int animalId);

        Task<string> GetAnimalsEthicsNumberDescription(int animalId);

        Task<CageLocationHistory> GetAnimalsCageLocation(int animalId);

        Task<string> GetAnimalsCageLocationDescription(int animalId);

        DateTime? GetAnimalsSurgeryDate(int animalId);

        void AddPhotoToAnimal(int animalId);

        void RemovePhotoFromAnimal(int animalId);

        Task<string> GetAnimalsListOfSurgeries(int animalId);

        Task<string> GetAnimalsSurgeon(int animalId);

        Task<string> GetAnimalsInjectionSurgeon(int animalId);

        DateTime? GetAnimalsInjectionDate(int animalId);

        IEnumerable<Animal> GetLivingAnimalsNotCheckedToday();

        IEnumerable<Animal> GetLivingAnimalsNotCheckedTodayForEmailing();

        IEnumerable<Animal> GetLivingAnimalsNotFedTodayForEmailing();

        IEnumerable<Animal> GetDeceasedAnimals();

        IEnumerable<Animal> GetInvestigatorsAnimals(string userName);

        IEnumerable<Animal> GetStudentsAnimals(string userName);

        Task CreateAnimal(Animal animal);

        Task<Animal> GetAnimalById(int id);

        Animal GetAnimalByUniqueId(string id);

        Task UpdateAnimal(Animal animal);

        Task DeleteAnimal(Animal animal);

        Task AddAnimalToEthicsNumber(int animalId, int ethicsNumberId);

        Task UpdateEthicsNumberHistoryForAnimalId(EthicsNumberHistory ethicsnumberhistory);

        int? GetAnimalsCurrentWeight(int animalId);

        Task<Animal> GetAnimalByTag(string animalId);

        Task Resurrect(int id);

        Task ReturnAnimalToStock(int id);

        Task BulkUpdateAnimals(List<Animal> animals, Grading? grading, Manipulation? manipulation, int? ArrivalStatus_Id);
    }
}