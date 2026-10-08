using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcxiomCRM.Data;
using AcxiomCRM.Models;
using System.Security.Claims;

namespace AcxiomCRM.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var customers = from c in _context.Customers select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.CustomerName.Contains(searchString) || 
                                                 c.Email.Contains(searchString) || 
                                                 c.Phone.Contains(searchString) || 
                                                 c.CompanyName.Contains(searchString));
            }

            return View(await customers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _context.Customers.AnyAsync(c => c.Email == customer.Email);
                var phoneExists = await _context.Customers.AnyAsync(c => c.Phone == customer.Phone);

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(customer);
                }

                if (phoneExists)
                {
                    ModelState.AddModelError("Phone", "Phone number already exists.");
                    return View(customer);
                }

                customer.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(customer);
                await _context.SaveChangesAsync();

                _context.AuditLogs.Add(new AuditLog
                {
                    UserId = customer.CreatedBy,
                    Action = "Create",
                    EntityName = "Customer",
                    RecordId = customer.CustomerId,
                    NewValue = customer.CustomerName,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Customers.AnyAsync(e => e.CustomerId == customer.CustomerId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}