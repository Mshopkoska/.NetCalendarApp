using CALENDAR_Version_3._0.Data;
using CALENDAR_Version_3._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CALENDAR_Version_3._0.Controllers.ActionFilters
{
    public class UserAccessOnly : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute, Microsoft.AspNetCore.Mvc.Filters.IActionFilter
    {
        private DAL _dal = new DAL();

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            // check if id is provided
            if (!context.RouteData.Values.ContainsKey("id")) return;
            int id = int.Parse((string)context.RouteData.Values["id"]);

            // check if user is auth
            if (context.HttpContext.User == null) return;
            var username = context.HttpContext.User.Identity.Name;
            if (username == null) return;

            // get event
            var myevent = _dal.GetEvent(id);
            
            // check if current user match with the event user
            if (myevent.User == null) return;
            if (myevent.User.UserName == username) return;

            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "NotFound" }));
        }
    }
}
