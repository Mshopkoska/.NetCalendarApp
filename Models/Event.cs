using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CALENDAR_Version_3._0.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public String ReminderFrequency { get; set; }

        [Required]
        public int NTimesFrequency { get; set; }

        //Relational data
        [Required]
        public virtual Location Location { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Event(IFormCollection form, Location location, String ReminderFrequency, ApplicationUser user)
        {
            User = user;
            Name = form["Event.Name"].ToString();
            Description = form["Event.Description"].ToString();
            StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
            EndTime = DateTime.Parse(form["Event.EndTime"].ToString());

            this.ReminderFrequency = ReminderFrequency;
            NTimesFrequency = int.Parse(form["Event.NTimesFrequency"].ToString());

            Location = location;
        }

        public void UpdateEvent(IFormCollection form, Location location, String ReminderFrequency, ApplicationUser user)
        {
            User = user;
            Name = form["Event.Name"].ToString();
            Description = form["Event.Description"].ToString();
            StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
            EndTime = DateTime.Parse(form["Event.EndTime"].ToString());

            this.ReminderFrequency = ReminderFrequency;
            NTimesFrequency = int.Parse(form["Event.NTimesFrequency"].ToString());

            Location = location;
        }

        public Event()
        {

        }
    }
}
