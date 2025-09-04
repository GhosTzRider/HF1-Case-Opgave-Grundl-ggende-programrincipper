using Plukliste;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebLagerSystem
{
    public class ConsoleToHTMLPluklist
    {
        public static string PluklisteManager()
        {
            return @"
                <div class=""box"">
                <div class=""block"">
                    Plukliste X af Y
                </div> 
                <div class=""block"">
                    Filnavn:
                </div> 
                <div class=""block"">
                 Navn:
                    <br>Forsendelse:
                    <br>Adresse:
                </div> 
                <table class=""table"">
                    <thead>
                        <tr>
                            <th>Antal:</th>
                            <th>Type:</th>
                            <th>Produktnr.:</th>
                            <th>Navn:</th>
                      </thead>
                      <tbody>  

                      </tbody>
                    </thead>
                </table>
                <div class=""block""></div> 
                    <button class=""button is-danger"">Quit</button>
                    <button class=""button is-warning"">Afslut Plukseddel</button>
                    <button class=""button is-success"">Næste Plukseddel</button>
                    <button class=""button is-success"">Forrige Plukseddel</button>
                    <button class=""button is-info"">Genindlæs Pluksedler</button>
        </div>
            ";
        }

        public static async void PluklisteReader()
        {

            CSVScanner scanner = new CSVScanner();
            scanner.CSVreader();
            List<string> files;
            files = Directory.EnumerateFiles("export", "*.XML").ToList();

            foreach (string file in files)
            {
                FileStream fileStream = File.OpenRead(file);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Pluklist));
                var plukliste = (Pluklist?)xmlSerializer.Deserialize(fileStream);
                fileStream.Close();
                if (plukliste != null && plukliste.Lines != null)
                {
                    string jsonString = JsonSerializer.Serialize(plukliste.Lines);
                    string jsonPath = $"export\\{plukliste.Name}_products.json";
                    await File.WriteAllTextAsync(jsonPath, jsonString);
                }
            }

        }
    }
}
