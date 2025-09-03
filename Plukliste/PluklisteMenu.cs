using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    public class PluklisteMenu
    {
        public void PluklisteMenuMethod()
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



            KonsolMenu.KonsolMenuMethod();
        }
    }
}
