using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CALENDAR_Version_3._0.Models;

namespace CALENDAR_Version_3._0.Models.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public List<SelectListItem> Location = new List<SelectListItem>();
        public string LocationName { get; set; }
        public string UserId { get; set; }

       // public List<String> ReminderFrequency = new List<String>();

        public ReminderFrequency ReminderFrequency { get; set; }

        public EventViewModel(Event myevent, List<Location> locations, string userid)
        {
            Event = myevent;
            LocationName = myevent.Location.Name;
            UserId = userid;
            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }

            //Add reminder frequencies
            //ReminderFrequency.Add("daily");
           // ReminderFrequency.Add("weekly");
           // ReminderFrequency.Add("monthly");
            //ReminderFrequency.Add("yearly");

            ReminderFrequency = myevent.reminderFrequency;
        }

        public EventViewModel(List<Location> locations, string userid)
        {
            UserId = userid;
            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }
            
            //Add reminder frequencies
           // ReminderFrequency.Add(new SelectListItem() { Text = "daily" });
           // ReminderFrequency.Add(new SelectListItem() { Text = "weekly" });
           // ReminderFrequency.Add(new SelectListItem() { Text = "monthly" });
           // ReminderFrequency.Add(new SelectListItem() { Text = "yearly" });
        }

        public EventViewModel()
        {

        }

    }
}
