using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models;
using PPL3_Banhangonline.Models.Viewmodels;

namespace PPL3_Banhangonline.Controllers
{
    public class SellerController : Controller
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetString("Role");

            if (accountId == null || role?.ToLower() != "seller")
            {
                return RedirectToAction("Login", "Account");
            }

            var seller = _context.Sellers
                .Include(s => s.Shop)
                .FirstOrDefault(s => s.UserID == accountId);

            if (seller == null)
            {
                return Content("Không tìm thấy seller.");
            }

            if (seller.Shop == null)
            {
                return Content("Seller này chưa có cửa hàng.");
            }

            return View(seller.Shop);
        }

        public IActionResult ManageProducts()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetString("Role");

            if (accountId == null || role?.ToLower() != "seller")
            {
                return RedirectToAction("Login", "Account");
            }

            var seller = _context.Sellers
                .Include(s => s.Shop)
                .FirstOrDefault(s => s.UserID == accountId);

            if (seller == null || seller.Shop == null)
            {
                return Content("Không tìm thấy cửa hàng của seller.");
            }

            var products = _context.Products
                .Where(p => p.ShopID == seller.Shop.ShopID)
                .ToList();

            ViewBag.ShopName = seller.Shop.ShopName;

            return View(products);
        }

        [HttpGet]
        public IActionResult RegisterSeller()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetString("Role");

            if (accountId == null || role?.ToLower() != "customer")
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult RegisterSeller(RegisterSellerViewModel model)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetString("Role");

            if (accountId == null || role?.ToLower() != "customer")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra mật khẩu đúng không
            var account = _context.Account.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null || account.Password != model.Password)
            {
                ViewBag.Error = "Mật khẩu không đúng.";
                return View(model);
            }

            // Kiểm tra đã là seller chưa
            var existingSeller = _context.Sellers.FirstOrDefault(s => s.UserID == accountId);
            if (existingSeller != null)
            {
                ViewBag.Error = "Tài khoản này đã đăng ký seller rồi.";
                return View(model);
            }

            // Lấy thông tin từ Customer đã có
            var customer = _context.Customers.FirstOrDefault(c => c.UserID == accountId);
            if (customer == null)
            {
                ViewBag.Error = "Không tìm thấy thông tin khách hàng.";
                return View(model);
            }

            // Tạo Seller từ thông tin Customer
            var seller = new Seller
            {
                UserID = accountId.Value,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                Age = customer.Age
            };

            _context.Sellers.Add(seller);
            _context.SaveChanges();

            // Tạo Shop
            var shop = new Shop
            {
                SellerID = seller.SellerID,
                ShopName = model.ShopName
            };
            _context.Shops.Add(shop);

            // Đổi Role → seller
            account.Role = "seller";
            _context.SaveChanges();

            // Cập nhật session
            HttpContext.Session.SetString("Role", "seller");

            TempData["Success"] = "Đăng ký seller thành công!";
            return RedirectToAction("Index", "Seller");
        }
    }
}
