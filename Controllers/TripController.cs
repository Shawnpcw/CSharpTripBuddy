using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripBuddy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace TripBuddy.Controllers
{
    public class HomeController : Controller
    {
        private contextContext dbContext;
        public HomeController(contextContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Trip/createUser")]
        public IActionResult createUser(User newUser)
        {
            if(ModelState.IsValid)
            {              
                if ( dbContext.users.Any(u => u.username == newUser.username)){
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                System.Console.WriteLine(newUser.password.ToString());
                System.Console.WriteLine("--------------------------------------------------------------------------");
                dbContext.users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("id", newUser.userid);
                System.Console.WriteLine(newUser.userid);
                return RedirectToAction("Trips");
                // System.Console.WriteLine("*******************************************************************");
            }
            else{
                return View("Index", newUser);
            }
        }
        [HttpGet("login")]
        public IActionResult Login(){
            ViewBag.error = "";
            return View("Index");
        }
        [HttpPost("loginaction")]
        public IActionResult LoginAction(LoginUser userSubmission)
        {            
            // User newe = new User();
            // ModelBundle ViewBundle = new ModelBundle{ UserModel = newe, LoginUserModel = userSubmission };
            if(ModelState.IsValid)
            {
                System.Console.WriteLine("-----------------------------------------------------------------");
                System.Console.WriteLine("Does this work? Valid");
                System.Console.WriteLine(userSubmission.Username2);
                System.Console.WriteLine("-----------------------------------------------------------------");
                var userInDb = dbContext.users.FirstOrDefault(u => u.username == userSubmission.Username2);
                if(userInDb == null)
                {             
                    System.Console.WriteLine("-----------------------------------------------------------------");
                    System.Console.WriteLine("Does this work? null");
                    System.Console.WriteLine("-----------------------------------------------------------------");
                    ViewBag.error = "UserName is not in DB";

                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.password, userSubmission.password);
                if(result == 0)
                {
                    System.Console.WriteLine("-----------------------------------------------------------------");
                    System.Console.WriteLine("Does this work? hash didnt work");
                    System.Console.WriteLine("-----------------------------------------------------------------");
                    ViewBag.error = "Password Does not match";

                    return View("Index");
                }
                

                HttpContext.Session.SetInt32("id", userInDb.userid);
                System.Console.WriteLine("-----------------------------------------------------------------");
                System.Console.WriteLine("Does this work? Putting in session");
                System.Console.WriteLine("-----------------------------------------------------------------");
                return RedirectToAction("Trips");
            } 
            return View("Index"); 
            
        }
        [HttpGet("Trips")]
        public IActionResult Trips(){
            if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Index");
                
            }
            int userid = (int)HttpContext.Session.GetInt32("id");
            User user = dbContext.users.SingleOrDefault(d=>d.userid == (int)userid);
            ViewBag.name = user.name;
            // var yourTrips = dbContext.users.Where(i=>i.userid == userid).Include(p=>p.TripMates).ThenInclude(p=>p.Trip).ToList();
            // // var yourTrips = dbContext.users.Where(i=>i.userid != userid).Include(p=>p.TripMates).ThenInclude(p=>p.Trip).ToList();
            // ViewBag.myTrips = dbContext.trips.Where(i=>i.plannerid == userid).ToList();
            // ViewBag.notMyTrips = dbContext.trips.Where(i=>i.plannerid != userid).ToList();

System.Console.WriteLine(userid);
            ViewBag.MyTrips = dbContext.tripmates.Include(i=>i.User).Include(j=>j.Trip).Where(k=>k.Trip.plannerid==userid).ToList();
            ViewBag.NotMyTrips = dbContext.tripmates.Include(i=>i.User).Include(i=>i.Trip).Where(i=>i.userid != userid && i.Trip.plannerid != userid).ToList();
            ViewBag.MyAddedTrips = dbContext.tripmates.Include(i=>i.User).Include(i=>i.Trip).Where(i=>i.userid == userid && i.Trip.plannerid != userid).ToList();
    

            // ViewBag.MyTrips = MyTrips;
            // ViewBag.NotMyTrips = NotMyTrips;
            // ViewBag.MyAddedTrips = MyAddedTrips;
   
            // foreach(var i in yourTrips){
            //     System.Console.WriteLine("-----------------------------------------------------------------1");
            //     foreach(var x in i.TripMates){
            //     System.Console.WriteLine("-----------------------------------------------------------------2");
            //     System.Console.WriteLine(x.Trip.destination);
            //     System.Console.WriteLine("-----------------------------------------------------------------2");
            //     }
            // }
            
            // ViewBag.othersTrips;
            // System.Console.WriteLine(user.ToString());
            return View();
        }
        
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("makeTrip")]
        public IActionResult makeTrip(){
             if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Index");
            }
            ViewBag.error = "false";

            return View();
        }
        [HttpPost("Trip/makeTrip")]
        public IActionResult makeTrip(Trip newTrip){
           
            if(ModelState.IsValid){
                //create
                DateTime now = DateTime.Now;
                if(newTrip.from < now || newTrip.to < now){
                    ViewBag.error = "Start and end Date need to start after today";
                    return View(newTrip);

                }
                if( newTrip.from >newTrip.to){
                    ViewBag.error = "You must end the Trip after you start the trip";

                    return View(newTrip);
                }
                newTrip.plannerid =  (int)HttpContext.Session.GetInt32("id");
                dbContext.trips.Add(newTrip);
                dbContext.SaveChanges();

                return RedirectToAction("Trips");
            }
            return View(newTrip);
        }

        
    }
}
