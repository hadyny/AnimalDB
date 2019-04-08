using System.Collections.Generic;

namespace AnimalDB.Repo.Entities
{
    public class ChargeCode
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}