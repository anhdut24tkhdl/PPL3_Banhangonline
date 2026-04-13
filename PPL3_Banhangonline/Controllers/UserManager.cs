using Microsoft.AspNetCore.Mvc;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models;

namespace PPL3_Banhangonline.Controllers
{
    [Controller]
    public class UserManager : Controller
    {

        private readonly AppDbContext _context;

        public UserManager(AppDbContext context)
        {
            _context = context;
        }

        private IActionResult? KiemTraAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role?.ToLower() != "admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return null;
        }

        public IActionResult Index()
        {
            var check = KiemTraAdmin();
            if (check != null)
            {
                return check;
            }

            var users = _context.Account.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var check = KiemTraAdmin();
            if (check != null)
            {
                return check;
            }

            var user = _context.Account.FirstOrDefault(x => x.AccountId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(Account model)
        {
            var check = KiemTraAdmin();
            if (check != null)
            {
                return check;
            }

            var user = _context.Account.FirstOrDefault(x => x.AccountId == model.AccountId);
            if (user == null)
            {
                return NotFound();
            }

            user.AccountName = model.AccountName;
            user.Role = model.Role;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật tài khoản thành công";
            return RedirectToAction("Index");
        }
    }
}
