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
        function attachPluklisteBoxListener() {{
  const box = document.getElementById('plukliste-box');
  if (!box) return;

  box.addEventListener('click', function (e) {{
    const action = e.target.getAttribute('data-action');
    if (action === 'naeste' || action === 'forrige' || action === 'afslut') {{
      let index = parseInt(box.getAttribute('data-index')) || 0;

      // Choose URL based on action
      let url = '/plukliste/action';
     if (action === 'afslut') {{
  url = '/plukliste/afslut';
      }}

      fetch(url, {{
        method: 'POST',
        headers: {{ 'Content-Type': 'application/json' }},
        body: JSON.stringify({{ action, index }})
      }})
      .then(response => response.text())
      .then(html => {{
  document.getElementById('plukliste-manager-content').innerHTML = html;
  

        // Re-attach listeners
        attachPluklisteBoxListener();
      }});
    }}
  }});
}}

document.addEventListener('DOMContentLoaded', () => {{
  attachPluklisteBoxListener();
}});
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