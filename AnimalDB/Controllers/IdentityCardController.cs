using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class IdentityCardController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IAnimal _animals;

        public IdentityCardController()
        {
            this._animals = new AnimalRepo();
        }

        //
        // GET: /IdentityCard/Single
        public async Task<ActionResult> Single(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            DateTime? InjectionDate = animal
                                        .SurgicalNotes
                                        .SingleOrDefault(m => m.SurgeryType.Description == "Injection") == null
                ? (DateTime?)null
                : animal
                    .SurgicalNotes
                    .Single(m => m.SurgeryType.Description == "Injection")
                    .Timestamp;

            DateTime? PlugDate = animal
                                    .SurgicalNotes
                                    .SingleOrDefault(m => m.SurgeryType.Description == "Plug") == null
                ? (DateTime?)null
                : animal
                    .SurgicalNotes
                    .Single(m => m.SurgeryType.Description == "Plug")
                    .Timestamp;

            DateTime? SurgeryDate = animal
                                        .SurgicalNotes
                                        .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                ? (DateTime?)null
                : animal
                    .SurgicalNotes
                    .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                    .OrderBy(m => m.Timestamp)
                    .Last()
                    .Timestamp;

            var model = new Models.IdentityCardViewModel()
            {
                ArrivalDate = animal.ArrivalDate,
                BirthDate = animal.BirthDate,
                Id = animal.Id,
                InjectionDate = InjectionDate,
                PlugDate = PlugDate,
                Sex = animal.Sex.ToString(),
                SurgeryDate = SurgeryDate,
                UniqueAnimalId = animal.UniqueAnimalId,
                OffspringKept = animal.Offspring.Count(m => m.CauseOfDeath == null),
                Colour = animal.Colour?.Description,
                ApprovalNumber = animal.ApprovalNumber?.Description,
                Transgene = animal.Transgene?.Description,
                Tag = animal.Tag
            };

            model.Surgeon = animal
                                .SurgicalNotes
                                .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                ? null
                : animal
                    .SurgicalNotes
                    .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                    .OrderBy(m => m.Timestamp)
                    .Last()
                    .Surgeon;

            model.Investigator = animal.Investigator?.FullName;
            model.Researcher = animal.Researcher?.FullName;
            model.Strain = animal.Strain?.Description;

            model.EthicsNumber = animal
                                    .EthicsNumbers
                                    .OrderByDescending(m => m.Timestamp)
                                    .FirstOrDefault()?
                                    .EthicsNumber
                                    .Text;


            return View(model);
        }

        //
        // GET: /IdentityCard/SelectMultiple
        [Authorize(Roles = "Technician, Administrator")]
        public ActionResult SelectMultiple()
        {
            return View(_animals.GetLivingAnimals());
        }

        ////
        //// POST: /IdentityCard/Multiple
        //[HttpPost]
        //public async Task<ActionResult> Multiple(List<Animal> animals)
        //{
        //    List<Models.IdentityCardViewModel> model = new List<Models.IdentityCardViewModel>();
        //    Models.IdentityCardViewModel tmp;
        //    Animal tmpAnimal;
        //    DateTime? InjectionDate = null, PlugDate = null, SurgeryDate = null;

        //    foreach (var animal in animals.Where(m => m.Id != 0))
        //    {
        //        //tmpAnimal = _animals.GetAnimalByUniqueId(animal.UniqueAnimalId);
        //        tmpAnimal = await _animals.GetAnimalById(animal.Id);

        //        InjectionDate = tmpAnimal
        //                            .SurgicalNotes
        //                            .SingleOrDefault(m => m.SurgeryType.Description == "Injection") == null
        //        ? (DateTime?)null
        //        : tmpAnimal
        //            .SurgicalNotes
        //            .Single(m => m.SurgeryType.Description == "Injection")
        //            .Timestamp;

        //        PlugDate = tmpAnimal
        //                        .SurgicalNotes
        //                        .SingleOrDefault(m => m.SurgeryType.Description == "Plug") == null
        //            ? (DateTime?)null
        //            : tmpAnimal
        //                .SurgicalNotes
        //                .Single(m => m.SurgeryType.Description == "Plug")
        //                .Timestamp;

        //        SurgeryDate = tmpAnimal
        //                            .SurgicalNotes
        //                            .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
        //            ? (DateTime?)null
        //            : tmpAnimal
        //                .SurgicalNotes
        //                .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
        //                .OrderBy(m => m.Timestamp)
        //                .Last()
        //                .Timestamp;

        //        tmp = new Models.IdentityCardViewModel()
        //        {
        //            ArrivalDate = tmpAnimal.ArrivalDate,
        //            BirthDate = tmpAnimal.BirthDate,
        //            Id = tmpAnimal.Id,
        //            InjectionDate = InjectionDate,
        //            PlugDate = PlugDate,
        //            Sex = tmpAnimal.Sex.ToString(),
        //            SurgeryDate = SurgeryDate,
        //            UniqueAnimalId = tmpAnimal.UniqueAnimalId,
        //            OffspringKept = animal.Offspring.Count(m => m.CauseOfDeath == null),
        //            Colour = tmpAnimal.Colour?.Description,
        //            ApprovalNumber = tmpAnimal.ApprovalNumber?.Description,
        //            Transgene = tmpAnimal.Transgene?.Description,
        //            Tag = tmpAnimal.Tag
        //        };

        //        tmp.Surgeon = tmpAnimal
        //                        .SurgicalNotes
        //                        .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
        //            ? null
        //            : tmpAnimal
        //                .SurgicalNotes
        //                .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
        //                .OrderBy(m => m.Timestamp)
        //                .Last()
        //                .Surgeon;


        //        tmp.Investigator = tmpAnimal.Investigator?.FullName;
        //        tmp.Researcher = tmpAnimal.Researcher?.FullName;
        //        tmp.Strain = tmpAnimal.Strain?.Description;
        //        tmp.EthicsNumber = tmpAnimal
        //                                .EthicsNumbers
        //                                .OrderByDescending(m => m.Timestamp)
        //                                .FirstOrDefault()?
        //                                .EthicsNumber
        //                                .Text;

        //        model.Add(tmp);
        //    }

        //    return View(model);
        //}


        //
        // POST: /IdentityCard/Multiple
        [HttpPost]
        public async Task<ActionResult> Multiple(List<int> animals, int? offspring)
        {
            List<Models.IdentityCardViewModel> model = new List<Models.IdentityCardViewModel>();
            List<Models.SmallIdentityCardViewModel> offspringToAdd = new List<Models.SmallIdentityCardViewModel>();
            Models.IdentityCardViewModel tmp;
            Animal tmpAnimal;
            DateTime? InjectionDate = null, PlugDate = null, SurgeryDate = null;

            foreach (var animal in animals)
            {
                //tmpAnimal = _animals.GetAnimalByUniqueId(animal.UniqueAnimalId);
                tmpAnimal = await _animals.GetAnimalById(animal);

                InjectionDate = tmpAnimal
                                    .SurgicalNotes
                                    .SingleOrDefault(m => m.SurgeryType.Description == "Injection") == null
                ? (DateTime?)null
                : tmpAnimal
                    .SurgicalNotes
                    .Single(m => m.SurgeryType.Description == "Injection")
                    .Timestamp;

                PlugDate = tmpAnimal
                                .SurgicalNotes
                                .SingleOrDefault(m => m.SurgeryType.Description == "Plug") == null
                    ? (DateTime?)null
                    : tmpAnimal
                        .SurgicalNotes
                        .Single(m => m.SurgeryType.Description == "Plug")
                        .Timestamp;

                SurgeryDate = tmpAnimal
                                    .SurgicalNotes
                                    .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                    ? (DateTime?)null
                    : tmpAnimal
                        .SurgicalNotes
                        .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                        .OrderBy(m => m.Timestamp)
                        .Last()
                        .Timestamp;

                tmp = new Models.IdentityCardViewModel()
                {
                    ArrivalDate = tmpAnimal.ArrivalDate,
                    BirthDate = tmpAnimal.BirthDate,
                    Id = tmpAnimal.Id,
                    InjectionDate = InjectionDate,
                    PlugDate = PlugDate,
                    Sex = tmpAnimal.Sex.ToString(),
                    SurgeryDate = SurgeryDate,
                    UniqueAnimalId = tmpAnimal.UniqueAnimalId,
                    OffspringKept = tmpAnimal.Offspring.Count(m => m.CauseOfDeath == null),
                    Colour = tmpAnimal.Colour?.Description,
                    ApprovalNumber = tmpAnimal.ApprovalNumber?.Description,
                    Transgene = tmpAnimal.Transgene?.Description,
                    Tag = tmpAnimal.Tag
                };

                tmp.Surgeon = tmpAnimal
                                .SurgicalNotes
                                .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                    ? null
                    : tmpAnimal
                        .SurgicalNotes
                        .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                        .OrderBy(m => m.Timestamp)
                        .Last()
                        .Surgeon;


                tmp.Investigator = tmpAnimal.Investigator?.FullName;
                tmp.Researcher = tmpAnimal.Researcher?.FullName;
                tmp.Strain = tmpAnimal.Strain?.Description;
                tmp.EthicsNumber = tmpAnimal
                                        .EthicsNumbers
                                        .OrderByDescending(m => m.Timestamp)
                                        .FirstOrDefault()?
                                        .EthicsNumber
                                        .Text;

                model.Add(tmp);

                if (offspring.HasValue)
                {
                    foreach (var _offspring in _animals.GetLivingAnimals().Where(m => m.UniqueAnimalId.StartsWith(tmpAnimal.UniqueAnimalId + "-")))
                    {
                        InjectionDate = _offspring
                                    .SurgicalNotes
                                    .SingleOrDefault(m => m.SurgeryType.Description == "Injection") == null
                ? (DateTime?)null
                : _offspring
                    .SurgicalNotes
                    .Single(m => m.SurgeryType.Description == "Injection")
                    .Timestamp;

                        PlugDate = _offspring
                                        .SurgicalNotes
                                        .SingleOrDefault(m => m.SurgeryType.Description == "Plug") == null
                            ? (DateTime?)null
                            : _offspring
                                .SurgicalNotes
                                .Single(m => m.SurgeryType.Description == "Plug")
                                .Timestamp;

                        SurgeryDate = _offspring
                                            .SurgicalNotes
                                            .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                            ? (DateTime?)null
                            : _offspring
                                .SurgicalNotes
                                .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                                .OrderBy(m => m.Timestamp)
                                .Last()
                                .Timestamp;

                        tmp = new Models.IdentityCardViewModel()
                        {
                            ArrivalDate = _offspring.ArrivalDate,
                            BirthDate = _offspring.BirthDate,
                            Id = _offspring.Id,
                            InjectionDate = InjectionDate,
                            PlugDate = PlugDate,
                            Sex = _offspring.Sex.ToString(),
                            SurgeryDate = SurgeryDate,
                            UniqueAnimalId = _offspring.UniqueAnimalId,
                            OffspringKept = _offspring.Offspring.Count(m => m.CauseOfDeath == null),
                            Colour = _offspring.Colour?.Description,
                            ApprovalNumber = _offspring.ApprovalNumber?.Description,
                            Transgene = _offspring.Transgene?.Description,
                            Tag = _offspring.Tag
                        };

                        tmp.Surgeon = _offspring
                                        .SurgicalNotes
                                        .Count(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug") == 0
                            ? null
                            : _offspring
                                .SurgicalNotes
                                .Where(m => m.SurgeryType.Description != "Injection" && m.SurgeryType.Description != "Plug")
                                .OrderBy(m => m.Timestamp)
                                .Last()
                                .Surgeon;


                        tmp.Investigator = _offspring.Investigator?.FullName;
                        tmp.Researcher = _offspring.Researcher?.FullName;
                        tmp.Strain = _offspring.Strain?.Description;
                        tmp.EthicsNumber = _offspring
                                                .EthicsNumbers
                                                .OrderByDescending(m => m.Timestamp)
                                                .FirstOrDefault()?
                                                .EthicsNumber
                                                .Text;

                        model.Add(tmp);
                    }
                }
            }

            return View(model);
        }



        //
        // GET: /IdentityCard/SelectSmallCard
        [Authorize(Roles = "Technician, Administrator")]
        public ActionResult SelectSmallCard()
        {
            return View(_animals.GetLivingAnimals());
        }

        //
        // POST: /IdentityCard/MultipleSmall
        [HttpPost]
        public async Task<ActionResult> MultipleSmall(List<int> animals, int? offspring)
        {
            List<Models.SmallIdentityCardViewModel> model = new List<Models.SmallIdentityCardViewModel>();
            List<Models.SmallIdentityCardViewModel> offspringToAdd = new List<Models.SmallIdentityCardViewModel>();
            Animal tmpAnimal;

            foreach (var animal in animals)
            {
                //tmpAnimal = _animals.GetAnimalByUniqueId(animal.UniqueAnimalId);
                tmpAnimal = await _animals.GetAnimalById(animal);
                
                model.Add(new Models.SmallIdentityCardViewModel()
                {
                    UniqueAnimalId = tmpAnimal.UniqueAnimalId,
                    BirthDate = tmpAnimal.BirthDate,
                    ArrivalDate = tmpAnimal.ArrivalDate,
                    Strain = tmpAnimal.Strain?.Description
                });

                if (offspring.HasValue)
                {
                    foreach (var _offspring in _animals.GetLivingAnimals().Where(m => m.UniqueAnimalId.StartsWith(tmpAnimal.UniqueAnimalId + "-")))
                    {
                        offspringToAdd.Add(new Models.SmallIdentityCardViewModel()
                        {
                            UniqueAnimalId = _offspring.UniqueAnimalId,
                            BirthDate = _offspring.BirthDate,
                            ArrivalDate = _offspring.ArrivalDate,
                            Strain = _offspring.Strain?.Description
                        });
                    }
                }
            }

            foreach(var _offspring in offspringToAdd)
            {
                if (model.Count(m => m.UniqueAnimalId == _offspring.UniqueAnimalId) == 0)
                {
                    model.Add(_offspring);
                }
            }

            return View(model.OrderBy(m => m.UniqueAnimalId));
        }
    }
}