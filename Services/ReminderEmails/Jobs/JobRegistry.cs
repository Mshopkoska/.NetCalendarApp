using FluentScheduler;
using CALENDAR_Version_3._0.Services.ReminderEmails.Jobs;


namespace CALENDAR_Version_3._0.Services.ReminderEmails
{
    public class JobRegistry : Registry
    {
        //send email reminders
        public JobRegistry()
        {
            Schedule<EventReminderEmailJob>().ToRunEvery(1).Days();
        }
    }
}
