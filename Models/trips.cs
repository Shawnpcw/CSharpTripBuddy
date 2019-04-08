using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TripBuddy.Models{
    public class Trip{
        public int tripid { get; set; }
        [MinLength(2)]
        [MaxLength(45)]
        [Required]
        public string destination { get; set; }
        [MaxLength(45)]
        [MinLength(2)]
        [Required]

        public string description { get; set; }
        [Required]
        public DateTime from { get; set; }
        [Required]
        public DateTime to { get; set; }
        public int plannerid { get; set; }

        public List<TripMate> TripMates { get; set; }
    }
}