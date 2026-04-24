using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPL3_Banhangonline.Database;
using PPL3_Banhangonline.Models;

public class RescueController : Controller
{
    private readonly AppDbContext _context;
    public RescueController(AppDbContext context) => _context = context;

    // 1. TRANG CHỦ GIẢI CỨU (Dành cho khách hàng xem)
    public async Task<IActionResult> Index()
    {
        var campaigns = await _context.RescueCampaigns
            .Include(c => c.Category)
            .Include(c => c.Shop)
            .Where(c => c.Status == "Active")
            .ToListAsync();
        return View(campaigns);
    }

    // 2. SELLER ĐĂNG KÝ CHIẾN DỊCH (GET)
    [HttpGet]
    public IActionResult CreateCampaign()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    // 2. SELLER ĐĂNG KÝ CHIẾN DỊCH (POST)
    [HttpPost]
    public async Task<IActionResult> CreateCampaign(RescueCampaign campaign)
    {
        var accountId = HttpContext.Session.GetInt32("AccountId");
        var seller = _context.Sellers.Include(s => s.Shop).FirstOrDefault(s => s.UserID == accountId);

        if (seller?.Shop != null)
        {
            campaign.ShopID = seller.Shop.ShopID;
            _context.RescueCampaigns.Add(campaign);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(campaign);
    }

    // 3. KHÁCH HÀNG ĐĂNG KÝ MUA (POST)
    [HttpPost]
    public async Task<IActionResult> RegisterBuy(int CampaignID, int Quantity)
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId"); // Giả sử bạn đã lưu khi login
        if (customerId == null) return RedirectToAction("Login", "Account");

        var campaign = await _context.RescueCampaigns.FindAsync(CampaignID);

        // Kiểm tra số lượng tối thiểu
        if (Quantity < campaign.MinQuantity)
        {
            TempData["Error"] = $"Bạn cần đăng ký mua tối thiểu {campaign.MinQuantity} sản phẩm.";
            return RedirectToAction("Index");
        }

        var reg = new RescueRegistration
        {
            CampaignID = CampaignID,
            CustomerID = customerId.Value,
            Quantity = Quantity
        };

        _context.RescueRegistrations.Add(reg);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đăng ký giải cứu thành công! Chúng tôi sẽ liên hệ khi thu hoạch.";
        return RedirectToAction("Index");
    }
    // 1. Trang hiện Form đăng ký
    public async Task<IActionResult> Register(int id)
    {
        var role = HttpContext.Session.GetString("Role");

        if (role != null && role.ToLower() == "seller")
        {
            // Gửi thông báo lỗi và quay về trang chủ
            TempData["Error"] = "Tài khoản người bán không thể thực hiện chức năng đăng ký giải cứu. Vui lòng sử dụng tài khoản khách hàng.";
            return RedirectToAction("Index", "Home");
        }  

        var campaign = await _context.RescueCampaigns
            .Include(c => c.Shop)
            .FirstOrDefaultAsync(m => m.CampaignID == id);

        if (campaign == null) return NotFound();

        return View(campaign);
    }

    // 2. Xử lý khi nhấn nút "Xác nhận đăng ký"
    [HttpPost]
    public async Task<IActionResult> ConfirmRegister(int campaignId, int quantity)
    {
        // 1. Kiểm tra Role và Login (Giữ nguyên của Cảm)
        var role = HttpContext.Session.GetString("Role");
        if (role != null && role.ToLower() == "seller")
        {
            TempData["Error"] = "Người bán không thể tham gia giải cứu!";
            return RedirectToAction("Index", "Home");
        }

        var customerId = HttpContext.Session.GetInt32("CustomerID");
        if (customerId == null) return RedirectToAction("Login", "Account");

        // 2. Kiểm tra Thời gian và Tính hợp lệ
        var campaign = await _context.RescueCampaigns.FindAsync(campaignId);

        if (campaign == null) return NotFound();

        // KIỂM TRA HẾT HẠN (QUAN TRỌNG)
        if (campaign.ExpectedHarvestDate.HasValue && campaign.ExpectedHarvestDate.Value <= DateTime.Now)
        {
            TempData["Error"] = "Rất tiếc! Chiến dịch này đã kết thúc thời gian đăng ký.";
            return RedirectToAction("Index", "Home");
        }

        if (quantity < campaign.MinQuantity)
        {
            TempData["Error"] = $"Số lượng tối thiểu là {campaign.MinQuantity}kg.";
            return RedirectToAction("Register", new { id = campaignId });
        }

        // 3. Lưu vào Database (Giữ nguyên của Cảm)
        var registration = new RescueRegistration
        {
            CampaignID = campaignId,
            CustomerID = customerId.Value,
            Quantity = quantity,
            RegistrationDate = DateTime.Now,
            Status = "Pending"
        };

        _context.RescueRegistrations.Add(registration);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Xác nhận chung tay thành công!";
        return RedirectToAction("Index", "Home");
    }
}