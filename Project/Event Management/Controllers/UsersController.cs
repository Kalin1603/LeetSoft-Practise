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
using System.Security.Claims;

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

        //Get: All Users
        public async Task<IActionResult> Index()
        {
            var userEvents = await _context.UserEvents
                .Include(ue => ue.User)
                .Include(ue => ue.Event)
                .ToListAsync();

            return View(userEvents);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                TempData["ErrorMessage"] = "Role 'User' does not exist. Please seed the database.";
                return RedirectToAction("Index", "Events");
            }

            var usersInRole = await _context.UserRoles
                .Where(ur => ur.RoleId == userRole.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var users = _userManager.Users
                .Where(u => usersInRole.Contains(u.Id))
                .ToList();

            var events = _context.Events.ToList();

            ViewData["Users"] = users ?? new List<User>();
            ViewData["Events"] = events ?? new List<Event>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EventId")] UserEvent userEvent)
        {
            if (!ModelState.IsValid)
            {
                var existingUserEvent = await _context.UserEvents
                    .FirstOrDefaultAsync(ue => ue.UserId == userEvent.UserId && ue.EventId == userEvent.EventId);

                if (existingUserEvent != null)
                {
                    // Ако съществува покана, проверяваме дали не е вече поканен
                    if (existingUserEvent.Status == "Invited")
                    {
                        TempData["ErrorMessage"] = "This user is already invited to this event.";
                    }
                    else
                    {
                        existingUserEvent.Status = "Invited";
                        _context.UserEvents.Update(existingUserEvent);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Create", "Users");
                }

                try
                {
                    userEvent.Status = "Invited";
                    _context.UserEvents.Add(userEvent);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "The invitation has been sent successfully.";
                    return RedirectToAction("Index", "Users"); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return View(userEvent);
                }
            }

            TempData["ErrorMessage"] = "Invalid form data.";
            return View(userEvent);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(ue => ue.User)
                .Include(ue => ue.Event)
                .FirstOrDefaultAsync(ue => ue.Id == id);

            if (userEvent == null)
            {
                return NotFound();
            }

            else
            {
                ViewData["EventId"] = new SelectList(
                    _context.Events.Select(e => new { e.Id, e.Title }),
                    "Id", "Title", userEvent.EventId);
            }
            return View(userEvent);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,UserId")] UserEvent userEvent)
        {
            if (id != userEvent.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var userEvents = await _context.UserEvents
                .Include(ue => ue.User)
                .Include(ue => ue.Event)
                .FirstOrDefaultAsync(ue => ue.UserId == userEvent.UserId && ue.EventId == userEvent.EventId);

                if (userEvents != null)
                {
                    TempData["ErrorMessage"] = "UserEvent with these IDs already exists!";
                    ViewData["EventId"] = new SelectList(
                        _context.Events.Select(e => new { e.Id, e.Title }),
                        "Id", "Title", userEvent.EventId
                    );
					userEvent.User = await _context.Users.FindAsync(userEvent.UserId);
					userEvent.Event = await _context.Events.FindAsync(userEvent.EventId);
                    return View(userEvent);
                }

                var existingUserEvent = await _context.UserEvents.FindAsync(id);

                if (existingUserEvent != null)
                {
                    existingUserEvent.EventId = userEvent.EventId;

                    _context.Update(existingUserEvent);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(
                _context.Events.Select(e => new { e.Id, e.Title }),
                "Id", "Title", userEvent.EventId
            );
			userEvent.User = await _context.Users.FindAsync(userEvent.UserId);
            userEvent.Event = await _context.Events.FindAsync(userEvent.EventId);

            return View(userEvent);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .Include(ue => ue.User)
                .Include(ue => ue.Event)
                .Where(ue => ue.User.UserName != "admin") 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }


        // GET: Users/Uninvite/5
        public async Task<IActionResult> Uninvite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEvent = await _context.UserEvents
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userEvent == null)
            {
                return NotFound();
            }

            userEvent.Status = "Uninvited";
            _context.UserEvents.Update(userEvent);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Stats()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Gets current logged user

            // Total events created by the user
            var totalEvents = _context.Events
                .Where(e => e.CreatorId == userId)
                .ToList();
            ViewBag.TotalEventsCount = totalEvents.Count;

            // Participants that confirmed their attendance over all invited
            var participants = _context.UserEvents
                .Where(ue => ue.Event.CreatorId == userId && (ue.Status == "Accepted" || ue.Status == "Invited"))
                .ToList();
            var participantsCount = participants.Count;
            var confirmedParticipantsCount = participants.Count(ue => ue.Status == "Accepted");
            ViewBag.ConfirmedParticipantsPercentage = (double)confirmedParticipantsCount / participantsCount * 100;

            return View();
        }


        private bool UserEventExists(int id)
        {
            return _context.UserEvents.Any(e => e.Id == id);
        }
    }
}
