//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace Plukliste;

class PluklisteProgram // test
{

    static void Main()
    {

        Directory.CreateDirectory("import");    // Starter med at lave et directory "import" - overskriver den gammel import?

        if (!Directory.Exists("export"))
        {
            Console.WriteLine("Directory \"export\" not found");        // Kigger på om export directory *ikke* findes
            Console.ReadLine();
            return;
        }

        Scanner.IScanner scanner = new CSVScanner();    // Opretter en instans af CSVScanner som implementerer IScanner interfacet
        scanner.CSVreader();                            // Kalder CSVreader metoden for at konvertere CSV filer til XML filer inden konsollen kaldes op
        Scanner.IScanner converter = new CSVScanner();  // gør det samme for CSVconverter metoden
        converter.CSVconverter();                       // og konverterer CSV filer i export til XML filer


        KonsolMenu.KonsolMenuMethod();                  // Kalder konsolmenu metoden
    }
}

