using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models.Viewmodels;

namespace PPL3_Banhangonline.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        private IActionResult? KiemTraCustomer()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetString("Role");

            if (accountId == null || role?.ToLower() != "customer")
            {
                return RedirectToAction("Login", "Account");
            }

            return null;
        }

        public IActionResult Orders(string? status)
        {
            if (KiemTraCustomer() != null)
            {
                return KiemTraCustomer()!;
            }

            var accountId = HttpContext.Session.GetInt32("AccountId")!.Value;
            var customer = _context.Customers.FirstOrDefault(c => c.UserID == accountId);
            if (customer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "Home");
            }

            var regularOrdersQuery = _context.Orders
                .Include(o => o.Payment)
                .Where(o => o.CustomerID == customer.CustomerID)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                regularOrdersQuery = regularOrdersQuery.Where(o => o.Status == status);
            }

            var rescueOrders = _context.RescueRegistrations
                .Include(r => r.Campaign)
                .Where(r => r.CustomerID == customer.CustomerID)
                .OrderByDescending(r => r.RegistrationDate)
                .ToList();

            var model = new CustomerOrdersViewModel
            {
                RegularOrders = regularOrdersQuery.ToList(),
                RescueRegistrations = rescueOrders,
                CurrentStatus = status
            };

            return View(model);
        }

        public IActionResult OrderDetail(int id)
        {
            if (KiemTraCustomer() != null)
            {
                return KiemTraCustomer()!;
            }

            var accountId = HttpContext.Session.GetInt32("AccountId")!.Value;
            var customer = _context.Customers.FirstOrDefault(c => c.UserID == accountId);
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.CustomerID != customer.CustomerID)
            {
                return RedirectToAction(nameof(Orders));
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            if (KiemTraCustomer() != null)
            {
                return KiemTraCustomer()!;
            }

            var accountId = HttpContext.Session.GetInt32("AccountId")!.Value;
            var customer = _context.Customers.FirstOrDefault(c => c.UserID == accountId);
            if (customer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "Home");
            }

            var model = new CustomerProfileViewModel
            {
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                Age = customer.Age
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(CustomerProfileViewModel model)
        {
            if (KiemTraCustomer() != null)
            {
                return KiemTraCustomer()!;
            }

            var accountId = HttpContext.Session.GetInt32("AccountId")!.Value;
            var customer = _context.Customers.FirstOrDefault(c => c.UserID == accountId);
            if (customer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailDaTonTai = _context.Customers
                .Any(c => c.CustomerID != customer.CustomerID && c.Email == model.Email && model.Email != null);
            if (emailDaTonTai)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }

            customer.Name = model.Name;
            customer.Phone = model.Phone;
            customer.Email = model.Email;
            customer.Address = model.Address;
            customer.Age = model.Age;
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công.";
            return RedirectToAction(nameof(EditProfile));
        }
    }
}
