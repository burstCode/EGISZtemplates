using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EGISZtemplates.Data;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using EGISZtemplates.Models;

namespace EGISZtemplates.Controllers
{
    [Authorize]
    public class TemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Показывает страничку и список с шаблонами
        public async Task<IActionResult> Manage()
        {
            var templates = await _context.Templates.ToListAsync();
            return View(templates);
        }

        // Тут происходит обновление выбранного шаблона
        // По кнопочке "Обновить шаблон"
        [HttpPost]
        public async Task<IActionResult> UpdateTemplate(int id, IFormFile file)
        {
            // Смотрим какой шаблон в таблице выбрал пользователь
            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }

            if (file != null && file.Length > 0)
            {
                // Логика сохранения файла
                var filePath = Path.Combine("Uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Обновление шаблона в базе данных
                template.TemplateFilename = file.FileName;
                template.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Шаблон успешно обновлен!";
            }

            return RedirectToAction(nameof(Manage));
        }

        // Тут работает кнопочка "Добавить шаблон"
        // Добавляется новый шаблончик короче
        [HttpPost]
        public async Task<IActionResult> AddTemplate(int id, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var template = new Template
                {
                    Id = id,
                    TemplateFilename = file.FileName,
                    LastUpdated = DateTime.Now
                };

                var filePath = Path.Combine("Uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _context.Templates.Add(template);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Шаблон успешно добавлен!";
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
