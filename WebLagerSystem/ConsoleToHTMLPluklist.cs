using Plukliste;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebLagerSystem
{
    public class ConsoleToHTMLPluklist
    {
        public static string PluklisteManager()
        {
            var files = Directory.EnumerateFiles("export", "*.JSON")
                .Select(f => f.Replace("export\\", ""))
                .ToList();

            var index = 0;
            FileStream file = File.OpenRead(Path.Combine("export", files[index]));
            var plukliste = JsonSerializer.Deserialize<Pluklist>(file);
            
            


            return $@"
                <div class=""box"">
                <div class=""block"">
                    Plukliste {index + 1} af {files.Count}               
                </div> 
                <div class=""block"">
                    Filnavn: {files[index]}
                </div> 
                <div class=""block"">
                    Navn: {plukliste?.Name}
                    <br>Forsendelse: {plukliste?.Forsendelse}
                    <br>Adresse: {plukliste?.Adresse}
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
                        <th>{}</th>
                        <th></th>
                        <th></th>
                        <th></th>
                      </tbody>
                    </thead>
                </table>
                <div class=""block""></div> 
                    <button class=""button is-danger"">Quit</button>
                    <button class=""button is-warning"">Afslut Plukseddel</button>
                    <button class=""button is-success"">N&aeligste Plukseddel</button>
                    <button class=""button is-success"">Forrige Plukseddel</button>
                    <button class=""button is-info"">Genindl&aeligs Pluksedler</button>
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
                    // Combine all relevant data into a single object
                    var exportData = new
                    {
                        Name = plukliste.Name,
                        Forsendelse = plukliste.Forsendelse,
                        Adresse = plukliste.Adresse,
                        Lines = plukliste.Lines
                    };

                    string json = JsonSerializer.Serialize(exportData);
                    string jsonPath = $"export\\{plukliste.Name}_products.json";
                    await File.WriteAllTextAsync(jsonPath, json);
                }
            }

        }
    }
}
