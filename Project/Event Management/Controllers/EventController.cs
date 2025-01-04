using Event_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management.Controllers
{
    public class EventController : Controller
    {
        private static List<Event> events = new List<Event>();
        private static int nextId = 1;

        // Показване на списъка със събития
        public IActionResult Index()
        {
            var sortedEvents = events.OrderBy(e => e.DateTime).ToList();
            return View(sortedEvents);
        }

        // Създаване на събитие (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Създаване на събитие (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event ev)
        {
            if (ModelState.IsValid)
            {
                ev.Id = nextId++;
                events.Add(ev);
                return RedirectToAction("Index");
            }
            return View(ev);
        }

        // Редактиране на събитие (GET)
        public IActionResult Edit(int id)
        {
            var ev = events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        // Редактиране на събитие (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Event ev)
        {
            if (ModelState.IsValid)
            {
                var existingEvent = events.FirstOrDefault(e => e.Id == ev.Id);
                if (existingEvent == null) return NotFound();

                existingEvent.Title = ev.Title;
                existingEvent.Description = ev.Description;
                existingEvent.DateTime = ev.DateTime;
                existingEvent.Location = ev.Location;

                return RedirectToAction("Index");
            }
            return View(ev);
        }

        // Изтриване на събитие (GET)
        public IActionResult Delete(int id)
        {
            var ev = events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        // Изтриване на събитие (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ev = events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();
            events.Remove(ev);
            return RedirectToAction("Index");
        }
    }
}
