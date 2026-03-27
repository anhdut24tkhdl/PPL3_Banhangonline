using Microsoft.AspNetCore.Mvc;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models;

namespace PPL3_Banhangonline.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Account model)
        {

            var user = _context.Account.FirstOrDefault(x =>
     x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
