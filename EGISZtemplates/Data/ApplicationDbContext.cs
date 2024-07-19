using Microsoft.EntityFrameworkCore;

namespace EGISZtemplates.Data
{
    public class ApplicationDbContext : DbContext
    {   // Контекст базы данных, ничего необычного
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; }
    }

    // Хэй-хэй, реализация класса шаблона для
    // взаимодействия с ним внутри программы! >:3
    public class Template
    {
        public int Id { get; set; }
        public string TemplateFilename { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
