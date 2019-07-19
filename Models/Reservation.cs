using System.ComponentModel.DataAnnotations;
using System;

namespace Exam.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId {get;set;}

        //Foreign Keys
        public int UserId {get;set;}
        public int EventId {get;set;}

        //Navagational Properties
        public User Guest {get;set;}
        public Event Attending {get;set;}


    }
}