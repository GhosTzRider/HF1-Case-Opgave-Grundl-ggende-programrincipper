using System;
using System.IO;

namespace Plukliste
{
    internal class HTMLReader
    {
        public static string ReadHTMLFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading HTML file: {ex.Message}");
                return string.Empty;
            }
        }

        public static string? GetTemplateForProduct(string productId)
        {
            if (productId.Contains("Print-welcome", StringComparison.OrdinalIgnoreCase))
                return "templates/PRINT-WELCOME.html";
            if (productId.Contains("Print-upgrade", StringComparison.OrdinalIgnoreCase))
                return "templates/PRINT-UPGRADE.html";
            if (productId.Contains("Print-opsigelse", StringComparison.OrdinalIgnoreCase))
                return "templates/PRINT-OPSIGELSE.html";
            // Add more mappings as needed
            return null;
        }
    }
}
