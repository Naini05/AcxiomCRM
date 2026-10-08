using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcxiomCRM.Data;
using AcxiomCRM.Models;
using System.Security.Claims;

namespace AcxiomCRM.Controllers
{
    [Authorize]
    public class OpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpportunityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var opportunities = _context.Opportunities.Include(o => o.Customer);
            return View(await opportunities.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Opportunity opportunity)
        {
            if (opportunity.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Opportunity Amount must be greater than 0.");
            }

            if (opportunity.Probability < 0 || opportunity.Probability > 100)
            {
                ModelState.AddModelError("Probability", "Probability must be between 0 and 100.");
            }

            if (opportunity.ExpectedCloseDate.Date < DateTime.Today && opportunity.Status == "Open")
            {
                ModelState.AddModelError("ExpectedCloseDate", "Expected Close Date cannot be in the past for an active opportunity.");
            }

            if (ModelState.IsValid)
            {
                opportunity.AssignedTo = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(opportunity);
                await _context.SaveChangesAsync();

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = opportunity.AssignedTo,
                    Action = "Create",
                    EntityName = "Opportunity",
                    RecordId = opportunity.OpportunityId,
                    NewValue = opportunity.OpportunityName,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(opportunity);
        }
    }
}