using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Plukliste
{
    internal class HTMLReader
    {
        public static List<string> FindMatchingProductIDs(string xmlFilePath)
        {
            var matches = new List<string>();
            var validProductIDs = new HashSet<string> { "PRINT-WELCOME", "PRINT-OPGRADE", "PRINT-OPSIGELSE" };

            using var file = File.OpenRead(xmlFilePath);
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));
            if (xmlSerializer.Deserialize(file) is Pluklist plukliste && plukliste.Lines != null)
            {
                foreach (var item in plukliste.Lines)
                {
                    if (validProductIDs.Contains(item.ProductID))
                    {
                        Console.WriteLine($"this product contains {item.ProductID}");
                        matches.Add(item.ProductID);
                    }
                }
            }
            return matches;
        }
    }
}
