using Plukliste;
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

        public static void PluklisteReader(Pluklist? plukliste)
        {
            List<string> files;
            files = [.. Directory.EnumerateFiles("export", "*.XML")];            // Bruger samme fremgangsmåde som i KonsolMenu til at finde XML-filer i "export" mappen
            plukliste = new Pluklist();            
            var index = -1;

            FileStream file = File.OpenRead(files[index]);                       // Læser filen som et index?
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Pluklist));   // Laver en ny klasse Pluklist 
            plukliste = xmlSerializer.Deserialize(file) as Pluklist;             // Definerer pluklisten som en deserialized fil fra ordren i XML-format

        }
    }
}
