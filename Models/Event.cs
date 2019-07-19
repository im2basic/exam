using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Exam.Models
{
    public class Event
    {
        [Key]
        public int EventId {get;set;}
        [Required]
        public string EventCoordinator {get;set;}

        [Required(ErrorMessage="Please enter a Title")]
        public string Title {get;set;}
        [Required(ErrorMessage="Please enter a Duration")]
        public string Duration {get;set;}
        [Required]
        [Display(Name="-")]
        public string DurationParameter {get;set;}
        [Required(ErrorMessage="Please enter a Description")]
        public string Description {get;set;}
        [Required]
        [Display(Name="Event Date/Time")]
        public DateTime EventTime {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        //FOREIGN KEYS HERE
        public int UserId {get;set;}
        //NAVAGATIONAL PROPERTY HERE
        public User Planner {get;set;}
        public List<Reservation> GuestList {get;set;}
    }

    //DATE VALIDATION HERE
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            if(value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid datetime!");
            }
            if(date < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future");
            }
            return ValidationResult.Success;
        }

    }
}