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


        KonsolMenu.KonsolMenuMethod();
     
    }
}

