using Microsoft.EntityFrameworkCore;

namespace EGISZtemplates.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; }
    }

    public class Template
    {
        public int Id { get; set; }
        public string TemplateFilename { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
