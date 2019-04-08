using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  AnimalDBCore.Core.Entities
{
    public class Transgene
    {
        public int Id { get;set; }

        [Display(Name = "Transgene ID")]
        public string Description { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}