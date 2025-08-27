using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Plukliste
{
    internal class Scanner

    {
        public interface IScanner
        {
            public abstract void CSVconverter(List<Item> scanliste, string fileName);

            public abstract void CSVreader();
        }
    }

    public class CSVScanner : Scanner.IScanner
    {
        public void CSVconverter(List<Item> scanliste, string fileName)     // Skal konvertere alle XML filer til CSV filer
        {
            var plukliste = new Pluklist();                                 // Opretter en ny instans af pluklisten
            plukliste.Forsendelse = "Pickup";                               // Sætter forsendelse til "Pickup"
            string trueName = fileName.Substring(fileName.IndexOf('_') + 1).Replace(".CSV", "");  // Fjerner alt før "_" og fjerner ".CSV" fra filnavnet
            plukliste.Name = trueName;                                      // Og definerer navnet på pluklisten som det rensede filnavn
            plukliste.Lines = scanliste;                                    // samt sætter linjerne i pluklisten til at være den scannede liste
            XmlSerializer serializer = new XmlSerializer(typeof(Pluklist)); // Opretter en ny XML serializer til pluklisten
            string path = $"export\\{fileName.Replace(".CSV", ".XML")}";    // Definerer stien til at være i export mappen, og ændrer filendelsen fra .CSV til .XML
            FileStream fileStream = File.OpenWrite(path);                   // Åbner en fil stream til at skrive til den definerede sti
            serializer.Serialize(fileStream, plukliste);                    // Serialiserer pluklisten til en XML fil
            fileStream.Close();
        }

        public void CSVreader()     // Skal konvertere alle CSV filer til XML filer
        {
            var files = Directory.EnumerateFiles("export", "*.CSV").ToList(); // Lister directory "export" og laver en liste af alle filerne i den mappe, og finder kun CSV filer
            List<Item> items;
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);       // Får kun filnavnet uden stien
                items = new List<Item>();                       // Opretter en ny liste af Item objekter
                var lines = File.ReadLines(file);
                foreach (var line in lines.Skip(1))             // Skip header line
                {
                    var columns = line.Split(';');              // Deler hver linje ved hvert semikolon
                    if (columns.Length >= 4)                    // Når linjerne er delt, og der er mindst 4 kolonner
                    {
                        var scanliste = new Item                // Lav en ny instans af ScannedItems klassen
                        {
                            ProductID = columns[0],
                            Type = (ItemType)Enum.Parse(typeof(ItemType), columns[1]),        // Byg listen op ved at parse kolonner til de rigtige datatyper
                            Title = columns[2],
                            Amount = int.Parse(columns[3])

                        };
                        items.Add(scanliste);
                    }

                }
                CSVconverter(items, fileName);
                File.Delete(file); // Sletter import mappen og alt indhold i den
            }

        }

    }
}
