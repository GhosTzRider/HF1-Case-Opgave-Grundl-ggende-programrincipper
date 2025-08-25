using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Plukliste
{
    internal partial class HTMLReader
    {
        public static List<string> FindMatchingProductIDs(string xmlFilePath)
        {
            var matches = new List<string>();
            var validProductIDs = new HashSet<string> { "PRINT-WELCOME", "PRINT-OPGRADE", "PRINT-OPSIGELSE" };

            using var file = File.OpenRead(xmlFilePath);
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
            if (xmlSerializer.Deserialize(file) is Pluklist plukliste && plukliste.Lines != null)
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
