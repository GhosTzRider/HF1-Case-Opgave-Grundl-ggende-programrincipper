using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    internal class ColoredLetters
    {
        public static void WriteLinesWithGreenLetter(string letters, char c) // Metode for grønt begyndelsesbogstav
        {
            var o = letters.IndexOf(c);
            Console.Write(letters.Substring(0, o));
            Console.ForegroundColor = ConsoleColor.Green; // Sætter farven til rød
            Console.Write(letters[o]);
            Console.ResetColor(); // Nulstiller farven til standard
            Console.WriteLine(letters.Substring(o + 1));
        }

        public static void WriteLinesWithRedLetter(string letters, char c) // Metode for rødt begyndelsesbogstav
        {
            var o = letters.IndexOf(c);
            Console.Write(letters.Substring(0, o));
            Console.ForegroundColor = ConsoleColor.Red; // Sætter farven til rød
            Console.Write(letters[o]);
            Console.ResetColor(); // Nulstiller farven til standard
            Console.WriteLine(letters.Substring(o + 1));
        }

        public static void WriteLinesOnlyInRed(string letters) // Metode for at skrive linje i rød
        {
            Console.ForegroundColor = ConsoleColor.Red; // Sætter farven til rød
            Console.WriteLine(letters);
            Console.ResetColor(); // Nulstiller farven til standard
        }

    }
}
