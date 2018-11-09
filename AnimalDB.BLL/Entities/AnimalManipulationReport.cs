using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class AnimalManipulationReport
    {
        public int Id { get; set; }

        [Required]
        public string Investigator_Id { get; set; }
        [ForeignKey("Investigator_Id")]
        public virtual Investigator Investigator { get; set; }

        [Display(Name = "Protocol #")]
        public string ProtocolNumber { get; set; }

        [Display(Name = "Beginning of Period")]
        public DateTime PeriodFrom { get; set; }
        [Display(Name = "End of Period")]
        public DateTime PeriodTo { get; set; }

        [Display(Name = "Animal Type")]
        public int Species_Id { get; set; }
        [ForeignKey("Species_Id")]
        public virtual Species Species { get; set; }

        /* Source of Animals */

        [Display(Name = "Breeding Unit")]
        [DefaultValue(0)]
        public int BreedingUnit { get; set; }
        [DefaultValue(0)]
        public int Commercial { get; set; }
        [DefaultValue(0)]
        public int Farm { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Born During Project")]
        public int BornDuringProject { get; set; }
        [DefaultValue(0)]
        public int Captured { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Imported into New Zealand")]
        public int Imported { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Public Sources")]
        public int PublicSources { get; set; }
        [Display(Name = "Total")]
        public int TotalSource { get; set; }

        /* Status of Animals */
        [DefaultValue(0)]
        [Display(Name = "Normal / Conventional")]
        public int NormalConventional { get; set; }
        [DefaultValue(0)]
        [Display(Name = "SPF / Germ Free")]
        public int SPFGermFree { get; set; }
        [DefaultValue(0)]
        public int Diseased { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Transgenic / Chimera")]
        public int TransgenicChimera { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Protected Species")]
        public int ProtectedSpecies { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Unborn / Pre-hatched")]
        public int UnbornPrehatched { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Other")]
        public int OtherStatus { get; set; }

        /* Manipulation */
        [DefaultValue(0)]
        public int Teaching { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Species Conservation")]
        public int SpeciesConservation { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Environmental Management")]
        public int EnvironmentalManagement { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Animal Husbandry")]
        public int AnimalHusbandry { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Basic Biological Research")]
        public int BasicBiologicalResearch { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Medical Research")]
        public int MedicalResearch { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Veterinary Research")]
        public int VeterinaryResearch { get; set; }
        [DefaultValue(0)]
        public int Testing { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Production of Biological Agents")]
        public int ProductionOfBiologicalAgents { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Development of Alternatives")]
        public int DevelopmentOfAlternatives { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Producing offspring with potential for compromised welfare")]
        public int ProducingOffspringWithPotentialForCompromisedWelfare { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Other")]
        public int OtherManipulation { get; set; }

        /* Re-use */
        [DefaultValue(0)]
        [Display(Name = "No Prior Use")]
        public int NoPriorUse { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Previously Used")]
        public int PreviouslyUsed { get; set; }

        /* Grading */
        [DefaultValue(0)]
        public int NoSufferingApproved { get; set; }
        [DefaultValue(0)]
        public int LittleSufferingApproved { get; set; }
        [DefaultValue(0)]
        public int ModerateSufferingApproved { get; set; }
        [DefaultValue(0)]
        public int SevereSufferingApproved { get; set; }
        [DefaultValue(0)]
        public int VerySevereSufferingApproved { get; set; }
        [DefaultValue(0)]
        [Display(Name = "No Impact")]
        public int NoSuffering { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Little Impact")]
        public int LittleSuffering { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Moderate Impact")]
        public int ModerateSuffering { get; set; }
        [DefaultValue(0)]
        [Display(Name = "High Impact")]
        public int SevereSuffering { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Very High Impact")]
        public int VerySevereSuffering { get; set; }

        /* Alive */
        [DefaultValue(0)]
        [Display(Name = "Retained [by your institution]")]
        public int RetainedByInstitution { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Returned [to owner]")]
        public int ReturnedToOwner { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Released [to the wild]")]
        public int ReleasedToTheWild { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Disposed of [to others e.g. works]")]
        public int DisposedOf { get; set; }
        [Display(Name = "Total Alive")]
        public int TotalAlive { get; set; }


        /* Dead */
        [DefaultValue(0)]
        [Display(Name = "Total Dead ")]
        public int TotalDead { get; set; }
        [Display(Name = "Total Used")]
        public int TotalUsed { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Nil Return")]
        public int NilReturn { get; set; }
        
        [Display(Name = "Completed by")]
        public string CompletedBy { get; set; }

        public string Designation { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime Timestamp { get; set; }
    }
}