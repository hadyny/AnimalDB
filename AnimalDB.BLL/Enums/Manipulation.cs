using System.ComponentModel.DataAnnotations;

namespace AnimalDB.Repo.Enums
{
    public enum Manipulation
    {
        Teaching,
        [Display(Name = "Species conservation")]
        SpeciesConservation,
        [Display(Name = "Environmental management")]
        EnvironmentalManagement,
        [Display(Name = "Animal husbandry")]
        AnimalHusbandry,
        [Display(Name = "Basic biological research")]
        BasicBiologicalResearch,
        [Display(Name = "Medical research")]
        MedicalResearch,
        [Display(Name = "Veterinary research")]
        VeterinaryResearch,
        Testing,
        [Display(Name = "Production of biological agents")]
        ProductionOfBiologicalAgents,
        [Display(Name = "Development of alternatives")]
        DevelopmentOfAlternatives,
        [Display(Name = "Producing offspring with potential for compromised welfare")]
        ProducingOffspringWithPotentialForCompromisedWelfare,
        Other
    }
}
