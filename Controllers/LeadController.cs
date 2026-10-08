using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcxiomCRM.Data;
using AcxiomCRM.Models;
using System.Security.Claims;

namespace AcxiomCRM.Controllers
{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var leads = from l in _context.Leads select l;

            if (!string.IsNullOrEmpty(searchString))
            {
                leads = leads.Where(l => l.LeadName.Contains(searchString) || 
                                         l.Email.Contains(searchString) || 
                                         l.Phone.Contains(searchString));
            }

            return View(await leads.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lead lead)
        {
            if (ModelState.IsValid)
            {
                lead.AssignedTo = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(lead);
                await _context.SaveChangesAsync();

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = lead.AssignedTo,
                    Action = "Create",
                    EntityName = "Lead",
                    RecordId = lead.LeadId,
                    NewValue = lead.LeadName,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(lead);
        }
    }
}