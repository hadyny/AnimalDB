using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalDB.Repo.Entities
{
    public class ChargeCode
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}