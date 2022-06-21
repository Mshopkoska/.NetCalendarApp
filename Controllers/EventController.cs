using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using CALENDAR_Version_3._0.Models;
using CALENDAR_Version_3._0.Data;
using CALENDAR_Version_3._0.Models.ViewModels;
using CALENDAR_Version_3._0.Controllers.ActionFilters;
using Microsoft.AspNetCore.Identity.UI.Services;




namespace CALENDAR_Version_3._0.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ApplicationDbContext _db;
        public IEmailSender _sender;
        private readonly ILogger<EventController> _logger;

        public EventController(IDAL _dal, UserManager<ApplicationUser> _usermanager, ApplicationDbContext _db, IEmailSender _sender, ILogger<EventController> _logger)
        {
            this._dal = _dal;
            this._usermanager = _usermanager;
            this._db = _db;
            this._sender = _sender;
            this._logger = _logger;
        }

        // GET: Event
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_dal.GetMyEvents(userid));
        }

        // GET: Event/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create

        public IActionResult Create()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new EventViewModel(_dal.GetMyLocations(userid), userid));
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EventViewModel vm, IFormCollection form)
        {
                var name = form["Event.Name"].ToString();
                var description = form["Event.Description"].ToString();
                var startTime = DateTime.Parse(form["Event.StartTime"].ToString());
                var endTime = DateTime.Parse(form["Event.EndTime"].ToString());
                var userid = form["UserId"].ToString();

                var locname = form["Location"].ToString();
                var location = _dal.GetLocation(locname);

                ReminderFrequency rf = (ReminderFrequency) int.Parse(form["Event.reminderFrequency"]);
                var NTimesFrequency = int.Parse(form["Event.NTimesFrequency"].ToString());

                var newevent = new Event(name,description,startTime,endTime,location, rf, NTimesFrequency, userid);

                _dal.CreateEvent(newevent);
                TempData["Alert"] = "Success! You created a new event for: " + form["Event.Name"];

                 
                newevent.eventReminderDate = CalculateEventReminderDate(newevent);
                
                var emails = form["Event.Emails"].ToString(); //comma separated 
                List<String> emailList = emails.Split(",").ToList();
                newevent.Emails = emailList;

            return RedirectToAction("Index");
        }

        public static DateTime CalculateEventReminderDate(Event e)
        {
            DateTime reminderDate = DateTime.Now;
            if (e.reminderFrequency.Equals(ReminderFrequency.Daily)) return reminderDate.AddDays(e.NTimesFrequency * 1);
            else if (e.reminderFrequency.Equals(ReminderFrequency.Weekly)) return reminderDate.AddDays(e.NTimesFrequency * 7);
            else if (e.reminderFrequency.Equals(ReminderFrequency.Monthly)) return reminderDate.AddMonths(e.NTimesFrequency * 1);
            else return reminderDate.AddYears(e.NTimesFrequency * 1);
        }

        

        // GET: Event/Edit/5
        
        [UserAccessOnly]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }
            var userid = _usermanager.GetUserId(User);
            var vm = new EventViewModel(@event, _dal.GetMyLocations(userid), userid);
            return View(vm);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
            try
            {
                var locname = form["Location"].ToString();
                var location = _db.Locations.FirstOrDefault(x => x.Name == locname);

                var reminderFrequency = form["ReminderFrequency"];
                var NTimesFrequency = int.Parse(form["Event.NTimesFrequency"].ToString());
                var name = form["Event.Name"].ToString();
                var description = form["Event.Description"].ToString();
                var startTime = DateTime.Parse(form["Event.StartTime"].ToString());
                var endTime = DateTime.Parse(form["Event.EndTime"].ToString());

                var eventid = int.Parse(form["Event.Id"]);
                var myevent = _db.Events.FirstOrDefault(x => x.Id == eventid);
                var user = _db.Users.FirstOrDefault(x => x.Id == form["UserId"].ToString());

                myevent.UpdateEvent(name, description, startTime, endTime, location, Models.ReminderFrequency.Daily, NTimesFrequency, user);
                
                _dal.UpdateEvent(myevent);
                TempData["Alert"] = "Success! You modified an event for: " + form["Event.Name"];


                myevent.eventReminderDate = CalculateEventReminderDate(myevent);

                var emails = form["Event.Emails"].ToString(); //comma separated 
                List<String> emailList = emails.Split(",").ToList();
                myevent.Emails = emailList;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                var userid = _usermanager.GetUserId(User);
                var vm = new EventViewModel(_dal.GetEvent(id), _dal.GetMyLocations(userid), _usermanager.GetUserId(User));
                return View(vm);
            }
        }

        // GET: Event/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteEvent(id);
            TempData["Alert"] = "You deleted an event.";
            return RedirectToAction(nameof(Index));
        }       
    }
}