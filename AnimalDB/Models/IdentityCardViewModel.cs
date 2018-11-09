using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnimalDB.Models
{
    public class IdentityCardViewModel
    {
        public int Id { get; set; }

        public string UniqueAnimalId { get; set; }

        public string Investigator { get; set; }

        public string Researcher { get; set; }

        [Display(Name = "AUP Number")]
        public string EthicsNumber { get; set; }

        public string Sex { get; set; }

        public string Strain { get; set; }

        [Display(Name = "Surgery Date")]
        public DateTime? SurgeryDate { get; set; }

        public string Surgeon { get; set; }

        [Display(Name = "Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Plug Date")]
        public DateTime? PlugDate { get; set; }

        [Display(Name = "Injection Date")]
        public DateTime? InjectionDate { get; set; }

        [Display(Name = "Offspring Kept")]
        public int OffspringKept { get; set; }

        public string Colour { get; set; }

        [Display(Name = "HSNO Act App#")]
        public string ApprovalNumber { get; set; }

        [Display(Name = "Transgene ID")]
        public string Transgene { get; set; }

        public string Tag { get; set; }
    }
}