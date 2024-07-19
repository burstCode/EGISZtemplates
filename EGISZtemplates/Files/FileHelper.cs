using System.IO;

namespace EGISZtemplates
{
    public static class FileHelper
    {
        // Проверяет наличие папки с загруженными файлами (шаблонами)
        // Если папочки нет, то создаёт её. Вызывается в начале Program.cs
        public static void EnsureUploadsFolderExists()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
