using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PPL3_Banhangonline.Database;

public class OrderController : Controller
{
    private readonly AppDbContext _context;
    public OrderController(AppDbContext context) { _context = context; }

    // Trang xem chi tiết một hóa đơn cụ thể
    public async Task<IActionResult> Invoice(int id)
    {
        var customerId = HttpContext.Session.GetInt32("AccountId"); // Lấy ID người dùng hiện tại
        if (customerId == null) return RedirectToAction("Login", "Account");

        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Payment)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderID == id);

        if (order == null) return NotFound();

        return View(order);
    }
}