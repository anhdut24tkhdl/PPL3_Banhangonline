using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PPL3_Banhangonline.Models;
using PPL3_Banhangonline.Database;

namespace PPL3_Banhangonline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Lấy danh sách danh mục (Code cũ của bạn)
            var categories = _context.Categories.ToList();

            // 2. Lấy danh sách chiến dịch giải cứu đang hoạt động (Active)
            // .ToList() để thực thi truy vấn
            var rescueItems = _context.RescueCampaigns
                                      .Where(c => c.Status == "Active")
                                      .OrderByDescending(c => c.CreatedAt) // Hiện cái mới nhất lên đầu
                                      .ToList();

            // 3. Đưa vào ViewBag để truyền sang View
            ViewBag.RescueItems = rescueItems;

            return View(categories); // Vẫn trả về categories làm Model chính
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
