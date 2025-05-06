using EventRegisterProject.Data;
using EventRegisterProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventRegisterProject.Controllers
{
    public class ParticipantsController : Controller
    {
        public IActionResult AddParticipant(int eventId)
        {
            int index = 0;
            var participants = InMemoryDatabase.Participants
                .Where(p => p.EventId == eventId)
                .ToList();
            if (participants.Count > 0) {
                index = participants.Max(p => p.Id) + 1;
            }
            ViewBag.ParticipantId = index;
            ViewBag.EventId = eventId;
            return View();
        }
        public IActionResult Create(Participant participant)
        {
            ViewBag.Message = "";
            if (!ModelState.IsValid) {
                ViewBag.Message = "Nie dodano wydarzenia - błąd walidacji.";
                return View("AddParticipant", participant.Id);
            }
            InMemoryDatabase.Participants.Add(participant);
            return RedirectToAction("Index", "Events");
        }

        public IActionResult ListByEvent(int eventId)
        {
            var participants = InMemoryDatabase.Participants
                .Where(p => p.EventId == eventId)
                .ToList();

            return View(participants);
        }

    }
}
