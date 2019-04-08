using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TripBuddy.Models;
using System.Linq;
namespace TripBuddy.Models
{
    public class User
    {
        [Key]
        public int userid {get;set;}

        [Required]
        [MinLength(2)]
        public string name {get;set;}
        [Required]
        [MinLength(2)]
        public string username {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [NotMapped]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string passwordConfirm { get; set; }
        
        public List<TripMate> TripMates {get;set;}
        public User(){
            TripMate TripMates = new TripMate();
        }
    }
    public class LoginUser
    {
        [Required]
        [MinLength(2)]
        public string Username2 {get; set;}
        [Required]
        public string password { get; set; }
    }
    
    public class ModelBundle
    {
        public User UserModel { get; set; }
        public LoginUser LoginUserModel { get; set; }
    }

}