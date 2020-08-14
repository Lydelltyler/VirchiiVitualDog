using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ninthMVP.Models;

namespace ninthMVP.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController (ILogger<HomeController> logger) {
            _logger = logger;
        }

        [HttpGet ("feed")]
        public IActionResult Feed () {
            Random random = new Random ();
            int num = random.Next (5, 11);
            int like = random.Next (1, 5);

            if (HttpContext.Session.GetInt32 ("meal") != 0) {
                int? meal = HttpContext.Session.GetInt32 ("meal");
                meal--;
                HttpContext.Session.SetInt32 ("meal", (int) meal);
                if (like != 3) {
                    int? fullness = HttpContext.Session.GetInt32 ("fullness");
                    fullness = fullness + num;
                    HttpContext.Session.SetInt32 ("fullness", (int) fullness);
                    string feedCap = HttpContext.Session.GetString ("caption");
                    HttpContext.Session.SetString ("caption", $"You feed your dog. fullness: {fullness}");
                }

            }
            return RedirectToAction ("Index");
        }

        [HttpGet ("play")]
        public IActionResult Play () {
            Random random = new Random ();
            int num = random.Next (5, 11);
            int like = random.Next (1, 5);
            int? energy = HttpContext.Session.GetInt32 ("energy");
            if (energy > 0) {
                if (like != 3) {
                    int? happiness = HttpContext.Session.GetInt32 ("happiness");
                    happiness = happiness + num;
                    HttpContext.Session.SetInt32 ("happiness", (int) happiness);
                    string feedCap = HttpContext.Session.GetString ("caption");
                    HttpContext.Session.SetString ("caption", $"Your dog gained happiness. happiness: {happiness}");
                } else {
                    HttpContext.Session.SetString ("caption", "No happiness Today");
                }

                energy = energy - 5;
                HttpContext.Session.SetInt32 ("energy", (int) energy);
            }
            return RedirectToAction ("Index");
        }

        [HttpGet ("work")]
        public IActionResult Work () {
            Random random = new Random ();
            int num = random.Next (1, 4);
            int? energy = HttpContext.Session.GetInt32 ("energy");
            if (energy > 0) {
                energy = energy - 5;
                HttpContext.Session.SetInt32 ("energy", (int) energy);

                int? meal = HttpContext.Session.GetInt32 ("meal");
                meal = meal + num;
                HttpContext.Session.SetInt32 ("meal", (int) meal);
            }

            return RedirectToAction ("Index");
        }

        [HttpGet ("sleep")]
        public IActionResult Sleep () {
            int? energy = HttpContext.Session.GetInt32 ("energy");
            energy = energy + 15;
            HttpContext.Session.SetInt32 ("energy", (int) energy);

            int? fullness = HttpContext.Session.GetInt32 ("fullness");
            fullness = fullness - 5;
            HttpContext.Session.SetInt32 ("fullness", (int) fullness);

            int? happiness = HttpContext.Session.GetInt32 ("happiness");
            happiness = happiness - 5;
            HttpContext.Session.SetInt32 ("happiness", (int) happiness);

            return RedirectToAction ("Index");
        }

        [HttpGet ("reset")]
        public IActionResult Reset () {
            HttpContext.Session.SetInt32 ("fullness", 20);
            HttpContext.Session.SetInt32 ("happiness", 20);
            HttpContext.Session.SetInt32 ("energy", 50);
            HttpContext.Session.SetInt32 ("meal", 3);

            ViewBag.Fullness = HttpContext.Session.GetInt32 ("fullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32 ("happiness");
            ViewBag.Energy = HttpContext.Session.GetInt32 ("energy");
            ViewBag.Meal = HttpContext.Session.GetInt32 ("meal");
            return View ("Index");
        }

        [HttpGet ("ending")]
        public IActionResult Ending () {
            ViewBag.Caption = HttpContext.Session.GetString ("caption");
            return View ();
        }

        [HttpGet ("")]
        public IActionResult Index () {

            int? energy = HttpContext.Session.GetInt32 ("energy");
            int? happiness = HttpContext.Session.GetInt32 ("happiness");
            int? fullness = HttpContext.Session.GetInt32 ("fullness");

            if (HttpContext.Session.GetInt32 ("fullness") == null) {
                HttpContext.Session.SetInt32 ("fullness", 20);
            }
            if (HttpContext.Session.GetInt32 ("happiness") == null) {
                HttpContext.Session.SetInt32 ("happiness", 20);
            }
            if (HttpContext.Session.GetInt32 ("energy") == null) {
                HttpContext.Session.SetInt32 ("energy", 50);
            }
            if (HttpContext.Session.GetInt32 ("meal") == null) {
                HttpContext.Session.SetInt32 ("meal", 3);
            }
            if (HttpContext.Session.GetString ("caption") == null) {
                HttpContext.Session.SetString ("caption", "Here we Go!");
            }

            if (happiness <= 0 || fullness <= 0) {
                HttpContext.Session.SetString ("caption", "Your Dog has passed away..");
                return RedirectToAction ("Ending");
            }

            if (happiness >= 100 && fullness >= 100 && energy >= 100) {
                HttpContext.Session.SetString ("caption", "Congratulations! You won!");
                return RedirectToAction ("Ending");
            }

            ViewBag.Fullness = HttpContext.Session.GetInt32 ("fullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32 ("happiness");
            ViewBag.Energy = HttpContext.Session.GetInt32 ("energy");
            ViewBag.Meal = HttpContext.Session.GetInt32 ("meal");
            ViewBag.Caption = HttpContext.Session.GetString ("caption");

            return View ();
        }

        [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error () {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}