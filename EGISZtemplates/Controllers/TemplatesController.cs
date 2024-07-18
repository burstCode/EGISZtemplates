using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EGISZtemplates.Data;
using System.Threading.Tasks;

namespace EGISZtemplates.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Manage()
        {
            var templates = await _context.Templates.ToListAsync();
            return View(templates);
        }
    }
}
