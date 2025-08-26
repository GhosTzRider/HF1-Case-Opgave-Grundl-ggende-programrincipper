using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plukliste
{
    internal class Scanner

    {
        public interface IScanner
        {
            public abstract void Reader();

            public abstract void Converter();
        }
    }

    public class CSVScanner : Scanner.IScanner
    {
        public void Reader()    // Skal læse alle CSV filer i import mappen
        {
            

        }


        public void Converter()     // Skal konvertere alle CSV filer til XML filer
        {
            throw new NotImplementedException();
        }
    }

}
