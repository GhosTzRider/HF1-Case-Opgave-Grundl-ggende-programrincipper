using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Plukliste
{
    internal class KonsolMenu
    {
        public static void KonsolMenuMethod()
        {
            char readKey = ' ';
            List<string> files;     // nye filer defineres som en string-list
            var index = -1;         // index starter på -1                       

            files = Directory.EnumerateFiles("export", "*.XML").ToList();  // Lister directory "export" og laver en liste af alle filerne i den mappe, og finder kun XML filer

            var plukliste = new Pluklist();
            string templateType = string.Empty;

            //ACT
            while (readKey != 'Q')      // Så længe readKey ikke er 'Q' (quit) så fortsæt
            {
                if (files.Count == 0)   // Hvis der ikke er nogen filer i listen
                {
                    Console.WriteLine("No files found."); // Skriv "No files found" i konsollen

                }
                else
                {
                    if (index == -1) index = 0; // ellers, hvis index er -1, så sæt index til 0                                     

                    Console.WriteLine($"Plukliste {index + 1} af {files.Count}");   // og skriv "Plukliste X af Y" i konsollen, hvor X er index + 1 og Y er antallet af filer
                    Console.WriteLine($"\nfile: {files[index]}");

                    //read file
                    FileStream file = File.OpenRead(files[index]);                      // Læser filen som et index?
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Pluklist));   // Laver en ny klasse Pluklist 
                    plukliste = (Pluklist?)xmlSerializer.Deserialize(file);             // Definerer pluklisten som en deserialized fil fra ordren i XML-format

                    // string jsonString = JsonSerializer.Serialize(plukliste.Lines);
                    // File.AppendAllTextAsync( "export/products.json", jsonString);

                    //print plukliste
                    if (plukliste != null && plukliste.Lines != null)
                    {
                        Console.WriteLine("\n{0, -13}{1}", "Name:", plukliste.Name);                            // Print navn på pluklisten
                        Console.WriteLine("{0, -13}{1}", "Forsendelse:", plukliste.Forsendelse);                // Print forsendelse på pluklisten
                        Console.WriteLine("{0, -13}{1}", "Adresse:", plukliste.Adresse);                        // Print adresse på pluklisten                                                                            

                        Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");   // Print overskrifter for pluklisten
                        foreach (var item in plukliste.Lines)
                        {
                            Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);        // Spytter en linje ud for hver item i pluklisten
                            if (item.Type == ItemType.Print) templateType = item.ProductID;   // Hvis item typen er fysisk, så sæt templateType til "Fysisk", ellers sæt den til "Print"
                        }
                    }
                    file.Close();       // Og lukker når den er done
                }

                //Print options - Interaktivt interface i konsollen
                Console.WriteLine("\n\nOptions:");
                ColoredLetters.WriteLinesWithRedLetter("Quit", 'Q'); // Quit skrives i rød, fordi det indikerer man afslutter programmet
                if (index >= 0)
                {
                    ColoredLetters.WriteLinesWithGreenLetter("Afslut plukseddel", 'A'); // Kalder metoderne for at skrive linjen med grønt begyndelsesbogstav
                }
                if (index > 0)
                {
                    ColoredLetters.WriteLinesWithGreenLetter("Forrige plukseddel", 'F');
                }
                if (index < files.Count - 1)
                {
                    ColoredLetters.WriteLinesWithGreenLetter("Næste plukseddel", 'N');
                }
                ColoredLetters.WriteLinesWithGreenLetter("Genindlæs pluklister", 'G');

                readKey = Console.ReadKey().KeyChar;
                if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper 
                Console.Clear();

                switch (readKey)    // switch kigger på hvilke taster der er trykket på og eksekverer en kommando ud fra det indtastede bogstav
                {
                    case 'G':
                        files = Directory.EnumerateFiles("export", "*.XML").ToList();
                        index = -1;
                        ColoredLetters.WriteLinesOnlyInRed("Pluklister genindlæst.");
                        break;
                    case 'F':
                        if (index > 0) index--;
                        break;
                    case 'N':
                        if (index < files.Count - 1) index++;
                        break;
                    case 'A':
                        //Move files to import directory
                        var filewithoutPath = files[index].Substring(files[index].LastIndexOf('\\'));   // Fjerner stien fra filnavnet, så kun selve filnavnet er tilbage
                        var destPath = string.Format(@"import\\{0}", filewithoutPath);                  // Definerer en ny streng som er stien til import-mappen med filnavnet

                        // Håndterer HTML vejledninger
                        string html = HTMLReader.ReplaceTagsInHTML(plukliste, templateType);    // kalder på metoden for at erstatte tags i HTML-filen med værdier fra pluklisten                        
                        string HtmlFileName = filewithoutPath.Replace(".XML", ".HTML");         // Definerer ny streng som er navnet på forsendelsen med filtypen .html
                        Directory.CreateDirectory("print");                                     // Laver en mappe der hedder print, hvis den ikke findes
                        StreamWriter writer = new StreamWriter($"print\\{HtmlFileName}");       // Definerer en ny HTML-fil i import-mappen med navnet på forsendelsen og templateType
                        writer.Write(html);                                                     // Skriver indholdet til HTML-filen ved at kalde på metoden ReplaceTagsInHTML
                        writer.Flush();                                                         // Flusher writeren

                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);  // Sletter filen i import-mappen hvis den allerede findes for at undgå fejl
                        }

                        File.Move(files[index], destPath);                                      // Flytter filen til import-mappen
                        Console.WriteLine($"Plukseddel {files[index]} afsluttet.");             // Skriver i konsollen at pluksedlen er afsluttet
                        files.Remove(files[index]);
                        if (index == files.Count) index--;
                        break;
                }
            }
        }
    }
}
