using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TripBuddy.Models
{
    public class TripMate
    {
        [Key]
        public int tripmateid {get;set;}

        public int userid {get;set;}
        [ForeignKey("userid")]
        public User User {get;set;}
        public int tripid {get;set;}
        [ForeignKey("tripid")]
        public Trip Trip {get;set;}
        
       
    }
}