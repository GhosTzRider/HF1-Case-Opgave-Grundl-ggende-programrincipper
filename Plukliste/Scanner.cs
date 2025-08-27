using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plukliste
{
    internal class Scanner

    {
        public interface IScanner
        {
            public abstract void CSVconverter();

            public abstract void CSVreader();
        }
    }

    public class CSVScanner : Scanner.IScanner
    {
        public void CSVconverter()   // Skal konvertere alle XML filer til CSV filer
        {

        }

        public void CSVreader()     // Skal konvertere alle CSV filer til XML filer
        {
            var files = Directory.EnumerateFiles("export", "*.CSV").ToList(); // Lister directory "export" og laver en liste af alle filerne i den mappe, og finder kun CSV filer
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines.Skip(1))   // Skip header line
                {
                    var columns = line.Split(';'); // Deler hver linje ved hvert semikolon
                    if (columns.Length >= 4)        // Når linjerne er delt, og der er mindst 4 kolonner
                    {
                        var scanliste = new ScannedItems        // Lav en ny instans af ScannedItems klassen
                        {
                            ProductID = columns[0],
                            Pickup = (ItemType)Enum.Parse(typeof(ItemType), columns[1]),        // Byg listen op ved at parse kolonner til de rigtige datatyper
                            Description = columns[2],
                            Amount = int.Parse(columns[3])
                            
                        };
                        Console.WriteLine("{0,-20}{1,-10}{2,-30}{3}", scanliste.ProductID, scanliste.Pickup, scanliste.Description, scanliste.Amount);      // Placeholder
                    }
                }
            }

        }

    }
}
