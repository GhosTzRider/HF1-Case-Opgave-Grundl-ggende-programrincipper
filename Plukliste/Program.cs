//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace Plukliste;

class PluklisteProgram
{

    static void Main()      // Alt kører i main - Skal ud i nogle klasser med underliggende metoder... op et tidspunkt
    {
        //Arrange
        char readKey = ' ';     // Istedet for char bruger vi ReadLine () til at læse input
        List<string> files;     // nye filer defineres som en string-list
        var index = -1;         // index starter på -1
        var standardColor = Console.ForegroundColor;
        Directory.CreateDirectory("import");    // Starter med at lave et directory "import" - overskriver den gammel import?

        if (!Directory.Exists("export"))
        {
            Console.WriteLine("Directory \"export\" not found");        // Kigger på om export directory *ikke* findes
            Console.ReadLine();
            return;
        }
        files = Directory.EnumerateFiles("export").ToList();        // Lister directory "export" og laver en liste af alle filerne i den mappe

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
                System.Xml.Serialization.XmlSerializer xmlSerializer =              // Serializer XML-filen
                    new System.Xml.Serialization.XmlSerializer(typeof(Pluklist));   // Laver en ny klasse Pluklist 
                var plukliste = (Pluklist?)xmlSerializer.Deserialize(file);         // Definerer pluklisten som en deserialized fil fra ordren i XML-format

                //print plukliste
                if (plukliste != null && plukliste.Lines != null)
                {
                    Console.WriteLine("\n{0, -13}{1}", "Name:", plukliste.Name);        // Print navn på pluklisten
                    Console.WriteLine("{0, -13}{1}", "Forsendelse:", plukliste.Forsendelse);    // Print forsendelse på pluklisten
                    //TODO: Add adresse to screen print

                    Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");   // Print overskrifter for pluklisten
                    foreach (var item in plukliste.Lines)
                    {
                        Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);        // Spytter en linje ud for hver item i pluklisten
                    }
                }
                file.Close();       // Og lukker når den er done 
            }

            //Print options - Interaktivt interface i konsollen
            Console.WriteLine("\n\nOptions:");
            Console.ForegroundColor = ConsoleColor.Green;       // hvert forbogstav er farvet grønt - kan kodes bedre
            Console.Write("Q");
            Console.ForegroundColor = standardColor;
            Console.WriteLine("uit");
            if (index >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("A");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("fslut plukseddel");
            }
            if (index > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("F");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("orrige plukseddel");
            }
            if (index < files.Count - 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("N");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("æste plukseddel");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("G");
            Console.ForegroundColor = standardColor;
            Console.WriteLine("enindlæs pluksedler");

            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a') readKey -= (char)('a' - 'A'); //HACK: To upper - Kan man ikke bare bruge .ToUpper ?
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)    // switch kigger på hvilke taster der er trykket på og eksekverer en kommando ud fra det indtastede bogstav
            {
                case 'G':
                    files = Directory.EnumerateFiles("export").ToList();
                    index = -1;
                    Console.WriteLine("Pluklister genindlæst"); 
                    break;
                case 'F':
                    if (index > 0) index--;
                    break;
                case 'N':
                    if (index < files.Count - 1) index++;
                    break;
                case 'A':
                    //Move files to import directory
                    var filewithoutPath = files[index].Substring(files[index].LastIndexOf('\\'));
                    File.Move(files[index], string.Format(@"import\\{0}", filewithoutPath));
                    Console.WriteLine($"Plukseddel {files[index]} afsluttet.");
                    files.Remove(files[index]);
                    if (index == files.Count) index--;
                    break;
            }
            Console.ForegroundColor = standardColor; //reset color

        }
    }
}
