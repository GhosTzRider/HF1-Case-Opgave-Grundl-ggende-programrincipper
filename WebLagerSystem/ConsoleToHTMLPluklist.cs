using Plukliste;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebLagerSystem
{
    public class ConsoleToHTMLPluklist
    {
        public static string PluklisteManager(int index = 0, char readKey = ' ')
        {
            var files = Directory.EnumerateFiles("export", "*.JSON")
                .Select(f => f.Replace("export\\", ""))
                .ToList();            

            using var file = File.OpenRead(Path.Combine("export", files[index]));
            var plukliste = JsonSerializer.Deserialize<Pluklist>(file);
            var items = plukliste?.Lines ?? new List<Item>();

            var tableRows = string.Join("\n", items.Select(item =>
                $@"<tr>
                    <td>{item.Amount}</td>
                    <td>{item.Type}</td>
                    <td>{item.ProductID}</td>
                    <td>{item.Title}</td>
                </tr>"
            ));

            return $@"
                <div class=""box"" id=""plukliste-box"" data-index=""{index}"">                
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
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}
                    </tbody>
                </table>
                <div class=""block""></div>                    
                    <button class=""button is-warning"" data-action=""afslut"">Afslut Plukseddel</button>
                    <button class=""button is-success"" data-action=""naeste"">N&aeligste Plukseddel</button>
                    <button class=""button is-success"" data-action=""forrige"">Forrige Plukseddel</button>
                    <button class=""button is-info"" data-action=""genindlaes"">Genindl&aeligs Pluksedler</button>
                </div>  
<script>

</script>
               
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











