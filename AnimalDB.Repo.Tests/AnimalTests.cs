using System;
using System.Collections.Generic;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using System.Data.Entity;
using AnimalDB.Repo.Contexts;

namespace AnimalDB.Repo.Tests
{
    [TestClass]
    public class AnimalTests
    {
        private IAnimal _animals;

        public AnimalTests()
        {
            IQueryable<Animal> animals = new List<Animal>
            {
                new Animal() { UniqueAnimalId = "Animal1" },
                new Animal() { UniqueAnimalId = "Animal2" },
                new Animal() { UniqueAnimalId = "Animal3", CauseOfDeath = Enums.CauseOfDeathEnum.Culled, DeathDate = new DateTime(2018, 01, 01) },
            }
            .AsQueryable();

            var mockSet = new Mock<DbSet<Animal>>();
            mockSet.As<IQueryable<Animal>>().Setup(m => m.Provider).Returns(animals.Provider);
            mockSet.As<IQueryable<Animal>>().Setup(m => m.Expression).Returns(animals.Expression);
            mockSet.As<IQueryable<Animal>>().Setup(m => m.ElementType).Returns(animals.ElementType);
            mockSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.GetEnumerator());
            

            var mockContext = new Mock<IAnimalDBContext>();
            mockContext.Setup(c => c.Animals).Returns(mockSet.Object);

            var repository = new AnimalRepo(mockContext.Object);

            _animals = repository;
        }

        [TestMethod]
        public void TestThatGetAnimalsReturnsAListOfAnimals()
        {
            Assert.IsInstanceOfType(_animals.GetAllAnimals(), typeof(List<Animal>));
        }

        [TestMethod]
        public void TestThatGetLivingAnimalsReturnsOnlyLivingAnimals()
        {
            var livingAnimals = _animals.GetLivingAnimals();

            Assert.IsFalse(livingAnimals.Any(m => m.DeathDate != null));
        }
    }
}
