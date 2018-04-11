using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
namespace dojodachi.Controllers{
    public class HomeController: Controller{

            void setValue(string key,int value){
                HttpContext.Session.SetInt32(key,value);
                }
            int? getValue(string key){
                return HttpContext.Session.GetInt32(key);
            }
            Random rand = new Random();
        [HttpGet]
        [Route("")]
        public IActionResult Home(){
            if(getValue("energy") == null){
                setValue("energy", 50);
                setValue("meals", 3);
                setValue("fullness", 20);
                setValue("happiness", 20);
            }
            ViewBag.energy = getValue("energy");
            ViewBag.meals = getValue("meals");
            ViewBag.fullness = getValue("fullness");
            ViewBag.happiness = getValue("happiness"); 
            if((int)getValue("happiness") <= 0 || (int)getValue("fullness") <= 0){
                TempData["message"] = "Your Dojodachi is Dojodead.";
                TempData["status"] = "death.jpg";
                return View("Result");
            }
            else if((int)getValue("happiness") >= 100 && (int)getValue("energy") >= 100 && (int)getValue("fullness") >= 100){
                TempData["message"] = "You have won! Your Dojodachi survived, time to try a living pet (no cats)!";
                TempData["status"] = "victory.jpg";
                return View("Result");
            }           
            return View("Index");
        }
        [Route("feed")]
        public IActionResult Feed(){
            int fullness = (int) getValue("fullness");
            int meals = (int) getValue("meals");
            if(meals != 0){
                meals --;
                setValue("meals", meals);
                int like = rand.Next(1,5);
                if(like != 1){
                    int incFullness = rand.Next(5,11);
                    fullness += incFullness;
                    setValue("fullness", fullness);
                    TempData["message"] = $"You fed your Dojodachi! Fullness increased by {incFullness}";
                }
                else{
                    TempData["message"] = "Your Dojodachi has an eating disorder!";
                }
            }
            else if(meals == 0){
                TempData["message"] = "You ain't got no food left pimpin!";
            }
            return RedirectToAction("Home");
        }
        [Route("play")]
        public IActionResult Play(){
            int happiness = (int) getValue("happiness");
            int energy = (int) getValue("energy");
            int like = rand.Next(1,5);
            if(like != 1){
                int incHappiness = rand.Next(5,11);
                happiness += incHappiness;
                setValue("happiness", happiness);
                setValue("energy", (energy-5));
                TempData["message"] = $"You played with your Dojodachi! Happiness increased by {incHappiness}";
            }
            else if(like == 1){
                setValue("energy", (energy-5));
                TempData["message"] = "Your Dojodachi isn't in the mood for your games!";
            }
            return RedirectToAction("Home");
        }
        [Route("work")]
        public IActionResult Work(){
            int incMeal = rand.Next(1,4);
            int meals = (int) getValue("meals");
            int energy = (int) getValue("energy");
            setValue("meals", meals + incMeal);
            setValue("energy", (energy-5));
            TempData["message"] = $"Your Dojodachi worked and earned {incMeal} meals!";
            return RedirectToAction("Home");
        }
        [Route("sleep")]
        public IActionResult Sleep(){
            int energy = (int) getValue("energy");
            int fullness = (int) getValue("fullness");
            int happiness = (int) getValue("happiness");
            setValue("energy", energy+15);
            setValue("fullness", fullness-5);
            setValue("happiness", happiness-5);
            TempData["message"] = "Your Dojodachi slept, and earned 15 energy";
            return RedirectToAction("Home");
        }
        [Route("reset")]
        public IActionResult Reset(){
            HttpContext.Session.Clear();
            return RedirectToAction("Home");
        }
    }
}