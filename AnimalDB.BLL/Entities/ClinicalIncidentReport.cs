using AnimalDB.Repo.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class ClinicalIncidentReport
    {
        public ClinicalIncidentReport()
        {
            this.FollowupSignatureDate = DateTime.Now;
            this.ReportedByDate = DateTime.Now;
            this.TreatmentSignatureDate = DateTime.Now;
            this.VetEvaluationDate = DateTime.Now;
        }

        public int Id { get; set; }

        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public string EthicsNumber { get; set; }

        // Clinical Finding

        [Display(Name = "Moribund/dying")]
        public bool Dying { get; set; }

        public bool Diarrhoea { get; set; }

        [Display(Name = "Fight Wounds")]
        public bool FightWounds { get; set; }
        [Display(Name = "Location")]
        public string FightWoundsLocation { get; set; }

        [Display(Name = "Mass (tumour/abscess)")]
        public bool Mass { get; set; }
        [Display(Name = "Location")]
        public string MassLocation { get; set; }

        [Display(Name = "Loss of appetite/Loss of weight")]
        public bool Appetite { get; set; }

        [Display(Name = "Coughing/Respiratory noise")]
        public bool Respiratory { get; set; }

        public bool Prolapse { get; set; }

        [Display(Name = "Rough Coat/Alopecia")]
        public bool Alopecia { get; set; }

        public bool Lethargic { get; set; }

        public bool Malocclusion { get; set; }

        public bool Lame { get; set; }

        [Display(Name = "Eye Problem")]
        public bool EyeProblem { get; set; }

        [Display(Name = "Post Surgical Problem")]
        public bool PostSurgicalProblem { get; set; }
        [Display(Name = "Describe")]
        public string PostSurgicalProblemDescription { get; set; }

        public bool Other { get; set; }
        [Display(Name = "Describe")]
        public string OtherDescription { get; set; }

        public string Weight { get; set; }

        [Display(Name="Additional Relevant History")]
        [DataType(dataType: DataType.MultilineText)]
        public string AdditionalHistory { get; set; }

        [Display(Name = "Reported By")]
        public string ReportedBy { get; set; }
        [Display(Name = "Date")]
        public DateTime ReportedByDate { get; set; }

        [Display(Name = "Veterinary Evaluation")]
        [DataType(dataType: DataType.MultilineText)]
        public string VetEvaluation { get; set; }

        [Display(Name = "Signature")]
        public bool VetEvaluationSignature { get; set; }

        [Display(Name = "Date")]
        public DateTime VetEvaluationDate { get; set; }

        [Display(Name = "Treatment Required")]
        public TreatmentRequiredEnum TreatmentRequired { get; set; }

        [Display(Name = "Euthanasia Required")]
        public EuthanasiaRequiredEnum EuthanasiaRequired { get; set; }

        public string Treatment1 { get; set; }
        public string Dose1 { get; set; }
        public string Frequency1 { get; set; }
        public string Duration1 { get; set; }
        public string Comments1 { get; set; }

        public string Treatment2 { get; set; }
        public string Dose2 { get; set; }
        public string Frequency2 { get; set; }
        public string Duration2 { get; set; }
        public string Comments2 { get; set; }

        public string Treatment3 { get; set; }
        public string Dose3 { get; set; }
        public string Frequency3 { get; set; }
        public string Duration3 { get; set; }
        public string Comments3 { get; set; }

        public TreatmentPerformedByEnum? TreatmentPerformedBy { get; set; }
        
        [Display(Name = "Signature")]
        public bool TreatmentSignature { get; set; }
        [Display(Name = "Date")]
        public DateTime TreatmentSignatureDate { get; set; }

        [Display(Name="Follow-up Examination")]
        [DataType(dataType: DataType.MultilineText)]
        public string FollowupExamination { get; set; }

        [Display(Name = "Signature")]
        public bool FollowupSignature { get; set; }
        [Display(Name = "Date")]
        public DateTime FollowupSignatureDate { get; set; }

        public DateTime Timestamp { get; set; }
    }

}