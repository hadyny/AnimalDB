namespace AnimalDB.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PupsByEthicsViewModel
    {
        [Display(Name = "AUP Number")]
        public string Ethics { get; set; }

        [Display(Name = "Total Culled Males")]
        public int TotalMale { get; set; }

        [Display(Name = "Total Culled Females")]
        public int TotalFemale { get; set; }
        
    }
}