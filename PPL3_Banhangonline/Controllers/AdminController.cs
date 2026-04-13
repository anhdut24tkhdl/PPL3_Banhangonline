using Microsoft.AspNetCore.Mvc;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models.Viewmodels;

namespace PPL3_Banhangonline.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
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

            var model = new AdminDashboardViewModel
            {
                SoTaiKhoan = _context.Account.Count(),
                SoKhachHang = _context.Customers.Count(),
                SoSeller = _context.Sellers.Count(),
                SoDanhMuc = _context.Categories.Count(),
                SoSanPham = _context.Products.Count()
            };

            return View(model);
        }

        public IActionResult CategoryList()
        {
            var check = KiemTraAdmin();
            if (check != null)
            {
                return check;
            }

            return RedirectToAction("Index", "Categories");
        }

        public IActionResult UserList()
        {
            var check = KiemTraAdmin();
            if (check != null)
            {
                return check;
            }

            return RedirectToAction("Index", "UserManager");
        }
    }
}
