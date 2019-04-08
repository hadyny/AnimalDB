using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Web.Models
{
    public class BulkChangeAnimalViewModel
    {

        public List<Animal> Animals { get; set; }

        public Manipulation? Purpose { get; set; }

        public Grading? Grading { get; set; }

        public int? ArrivalStatus_Id { get; set; }
        [ForeignKey("ArrivalStatus_Id")]
        public virtual ArrivalStatus Status { get; set; }
    }
}