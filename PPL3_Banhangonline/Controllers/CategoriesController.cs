namespace PPL3_Banhangonline.Controllers
{
   
    using Microsoft.AspNetCore.Mvc;
    using PPL3_Banhangonline.Database;

    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }
    }
}
