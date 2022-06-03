using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CALENDAR_Version_3._0.Data;
using CALENDAR_Version_3._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CALENDAR_Version_3._0.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public LocationController(IDAL idal, UserManager<ApplicationUser> usermanager)
        {
            _dal = idal;
            _usermanager = usermanager;
        }

        // GET: Location
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_dal.GetMyLocations(userid));
        }

        // GET: Location/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = _dal.GetLocation((int)id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Location location)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    location.UserId = userId;


                    _dal.CreateLocation(location);
                    TempData["Alert"] = "Success! You created a location for: " + location.Name;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = "An error occurred: " + ex.Message;
                    return View(location);
                }

            }
            return View(location);
        }
    }
}
