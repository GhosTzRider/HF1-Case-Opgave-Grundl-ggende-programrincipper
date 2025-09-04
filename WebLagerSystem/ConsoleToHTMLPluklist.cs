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
                <div>
                    <h1 class='title'>Create Plukliste</h1>
                    <form id='pluklisteForm'>
                        <textarea class='textarea' name='pluklisteData' placeholder='Enter plukliste JSON here'></textarea>
                        <button class='button is-primary mt-2' type='submit'>Save Plukliste</button>
                    </form>
                    <script>
                        document.getElementById('pluklisteForm').addEventListener('submit', async function(e) {
                            e.preventDefault();
                            let data = document.querySelector('textarea[name=pluklisteData]').value;
                            let response = await fetch('/api/plukliste', {
                                method: 'POST',
                                headers: { 'Content-Type': 'application/json' },
                                body: data
                            });
                            if(response.ok) {
                                alert('Plukliste saved successfully!');
                            } else {
                                alert('Failed to save plukliste.');
                            }
                        });
                    </script>
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
