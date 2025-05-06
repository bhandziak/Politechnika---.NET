using EventRegisterProject.Data;
using EventRegisterProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventRegisterProject.Controllers
{
    public class EventsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var events = InMemoryDatabase.Events;
            return View(events);
        }
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Event eventObj)
        {
            ViewBag.Message = "";
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Nie dodano wydarzenia - błąd walidacji.";
                return View("AddEvent", eventObj);
            }

            InMemoryDatabase.Events.Add(eventObj);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int eventId)
        {
            var eventObj = InMemoryDatabase.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            return View("Details", eventObj);
        }
    }
}
