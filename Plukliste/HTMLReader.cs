using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Plukliste
{
    internal partial class HTMLReader
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

        public static string ReplaceTagsInHTML(Pluklist plukliste, string templateType)
        {
            string PList = "<ul>";   // Starter med en ul liste
            foreach (var item in plukliste.Lines)
            {
                PList += $"<li>{item.Amount} | {item.Title}</li> ";
            }
            PList += "</ul>";        // og afslutter med en ul liste
            string content = string.Empty;      // Starter med en tom streng
            try
            {
                using (StreamReader reader = new StreamReader($"templates\\{templateType}.html"))
                {
                    content = reader.ReadToEnd();       // forsøger at læse indholdet af HTML-filerne i templates mappen
                }
                content = content.Replace("[Name]", plukliste.Name)             // og erstatte tags i HTML-filen med værdier fra pluklisten
                                 .Replace("[Adresse]", plukliste.Adresse)
                                 .Replace("[Plukliste]", PList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error replacing tags in HTML: {ex.Message}");   // hvis en værdi ikke findes i pluklisten, fanges undtagelsen og en fejlmeddelelse vises
            }
            return content;
        }

    }
}
