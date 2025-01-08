using System.Diagnostics;
using System.Security.Claims;
using Event_Management.Data;
using Event_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Gets current logged user
            
            var invites = _context.UserEvents
                .Where(ue => ue.UserId == userId && ue.Status == "Invited")
                .Include(ue => ue.Event)
                .ToList();

            return View(invites);
        }

        public IActionResult Accept(int id)
        {
            var invite = _context.UserEvents.Find(id);
            invite.Status = "Accepted";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var invite = _context.UserEvents.Find(id);
            invite.Status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("Index");
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
    }
}
