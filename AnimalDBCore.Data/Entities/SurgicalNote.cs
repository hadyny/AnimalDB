using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AnimalDBCore.Core.Entities
{
    public class SurgicalNote
    {
        public SurgicalNote()
        {
            this.WellfareScores = new HashSet<SurgicalWelfareScore>();
        }

        public int Id { get; set; }

        [Display(Name = "Date of surgery")]
        public DateTime Timestamp { get; set; }

        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        [Required]
        public string Surgeon { get; set; }

        [Display(Name = "Surgery Type")]
        public int SurgeryType_Id { get; set; }
        [ForeignKey("SurgeryType_Id")]
        public virtual SurgeryType SurgeryType { get; set; }

        [Display(Name = "Virus Type")]
        public int? VirusType_Id { get; set; }
        [ForeignKey("VirusType_Id")]
        public virtual VirusType VirusType { get; set; }

        [Display(Name = "GD Timeline")]
        public int? GDTimeline_Id { get; set; }
        [ForeignKey("GDTimeline_Id")]
        public virtual GDTimeline GDTimeline { get; set; }

        public virtual ICollection<SurgicalWelfareScore> WellfareScores { get; set; }
    }

    public class VirusType
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class SurgicalWelfareScore
    {
        public int Id { get; set; }

        public int SurgicalNote_Id { get; set; }

        public virtual SurgicalNote SurgicalNote { get; set; }

        public DateTime Timestamp { get; set; }

        public int Day { get; set; }

        [Display(Name = "Body wt yesterday")]
        public int BodyWtYesterday { get; set; }

        [Display(Name = "Body wt today")]
        public int BodyWtToday { get; set; }

        [Display(Name = "Body wt change")]
        public int BodyWtChange
        {
            get
            {
                return this.BodyWtToday -  this.BodyWtYesterday;
            }

        }

        [Display(Name = "BAR (bright, alert, responsive")]
        public int BAR { get; set; }

        [Display(Name = "Approach response (inquisitive behaviour investigates your presence)")]
        public int ApproachResponse { get; set; }

        
        public int Inactive { get; set; }

        [Display(Name = "Hunched posture")]
        public int HunchedPosture { get; set; }

        [Display(Name = "Coat rough, fur on end")]
        public int CoatRough { get; set; }

        [Display(Name = "Red eye/nose discharges")]
        public int RedDischarges { get; set; }

        [Display(Name = "Pink staining of neck")]
        public int PinkStaining { get; set; }

        [Display(Name = "Dehydration - Skin turgor test")]
        public int Dehydration { get; set; }


        [Display(Name = "Back arch (Hunched up with arched back)")]
        public int BackArch { get; set; }

        [Display(Name = "Belly Press")]
        public int BellyPress { get; set; }

        [Display(Name = "Writhe (Twisting of body or flank)")]
        public int Writhe { get; set; }

        [Display(Name = "Stagger (Sudden loss of balance/gait)")]
        public int Stagger { get; set; }

        [Display(Name = "Twitch (Sudden spasm of flank muscles)")]
        public int Twitch { get; set; }

        [Display(Name = "Fall (Animal fall over)")]
        public int Fall { get; set; }


        [Display(Name = "Previous weight of bottle")]
        public int StartWeight { get; set; }

        [Display(Name = "Current weight of bottle")]
        public int CurrentWeight { get; set; }

        [Display(Name = "Water intake")]
        public int WaterIntake
        {
            get
            {
                return this.StartWeight - this.CurrentWeight;
            }
        }


        [Display(Name = "Wound OK")]
        public bool Wound { get; set; }

        public bool Bleeding { get; set; }

        [Display(Name = "Sutures/clips OK")]
        public bool SuturesClips { get; set; }


        public string Drug { get; set; }

        public string Dose { get; set; }

        [Display(Name = "Fluids by SC injection")]
        public string Fluids { get; set; }

        [Display(Name = "Other drugs")]
        public string OtherDrugs { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }
    }
}