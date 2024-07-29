using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EGISZtemplates.Data;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using EGISZtemplates.Models;
using EGISZtemplates.Files;

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
            
            if (file == null)
            {
                TempData["ErrorMessage"] = "Файл не выбран!";
                return RedirectToAction(nameof(Manage));
            }
            
            if (file.Length == 0)
            {
                TempData["ErrorMessage"] = "Загружен пустой файл!";
                return RedirectToAction(nameof(Manage));
            }

            try
            {
                var updatedTemplate = FilenameParser.ParseTemplateFilename(file.FileName);

                // Удаляем устаревший шаблон из файловой системы
                var oldFilePath = Path.Combine("TemplateFiles", template.TemplateFilename);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                // Удаляем его из базы данных
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();

                // Сохраняем новый шаблон в файловую систему
                var newfilePath = Path.Combine("TemplateFiles", file.FileName);
                using (var stream = new FileStream(newfilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // И сохраняем его в базу данных
                _context.Templates.Add(updatedTemplate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Шаблон успешно обновлен!";
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Manage));
        }

        // Тут работает кнопочка "Добавить шаблон"
        // Добавляется новый шаблончик короче
        [HttpPost]
        public async Task<IActionResult> AddTemplate(IFormFile file)
        {
            if (file == null)
            {
                TempData["ErrorMessage"] = "Файл не выбран!";
                return RedirectToAction(nameof(Manage));
            }

            if (file.Length == 0)
            {
                TempData["ErrorMessage"] = "Загружен пустой файл!";
                return RedirectToAction(nameof(Manage));
            }

            try
            {
                var template = FilenameParser.ParseTemplateFilename(file.FileName);

                // Проверяем шаблон на дубликат
                if (_context.Templates.Any(t => t.Id == template.Id))
                {
                    throw new ArgumentException("Шаблон с таким id уже существует!");
                }

                // Сохраняем новый шаблон в файловую систему
                var filePath = Path.Combine("TemplateFiles", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Добавляем новый шаблон в базу данных
                _context.Templates.Add(template);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Шаблон успешно добавлен!";
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}

