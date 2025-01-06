using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Management.Data;
using Event_Management.Models;
using Microsoft.AspNetCore.Identity;

namespace Event_Management.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

         public UsersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserEvents.Include(u => u.Event).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(u => u.Event)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var users = _userManager.Users.ToList();  // Извличаме потребителите от Identity
            var events = _context.Events.ToList();    // Извличаме събитията от базата

            // Проверка дали има потребители и събития
            ViewData["Users"] = users ?? new List<User>();
            ViewData["Events"] = events ?? new List<Event_Management.Models.Event>();

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,EventId,Status")] UserEvent userEvent)
        {
            if (ModelState.IsValid)  // Проверка дали моделът е валиден
            {
                // Задаваме статус на "Invited"
                userEvent.Status = "Invited";

                // Добавяме събитието към базата
                _context.Add(userEvent);
                await _context.SaveChangesAsync();

                // Показваме съобщение за успешната покана
                TempData["SuccessMessage"] = $"User {userEvent.User.FirstName} has been invited to the event {userEvent.Event.Title}.";

                // Пренасочваме към Index на събитията
                return RedirectToAction("Index", "Events");
            }

            // Ако има грешки, зареждаме отново събития и потребители в dropdown менютата
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "UserName", userEvent.UserId);
            return View(userEvent);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents.FindAsync(id);
            if (userEvent == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userEvent.UserId);
            return View(userEvent);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,EventId,Status")] UserEvent userEvent)
        {
            if (id != userEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserEventExists(userEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userEvent.UserId);
            return View(userEvent);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(u => u.Event)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userEvent = await _context.UserEvents.FindAsync(id);
            if (userEvent != null)
            {
                _context.UserEvents.Remove(userEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserEventExists(int id)
        {
            return _context.UserEvents.Any(e => e.Id == id);
        }
    }
}
