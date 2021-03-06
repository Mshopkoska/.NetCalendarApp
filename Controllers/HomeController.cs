using CALENDAR_Version_3._0.Models;
using CALENDAR_Version_3._0.Helpers;
using CALENDAR_Version_3._0.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace CALENDAR_Version_3._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDAL _idal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public HomeController(ILogger<HomeController> _logger, IDAL _idal, UserManager<ApplicationUser> _usermanager)
        {
            this._logger = _logger;
            this._idal = _idal;
            this._usermanager = _usermanager;
        }

        public IActionResult Index()
        {
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetEvents());
            return View();
        }

        [Authorize]
        public IActionResult MyCalendar()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetMyLocations(userid));
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetMyEvents(userid));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
