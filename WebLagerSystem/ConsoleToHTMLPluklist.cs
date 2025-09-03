namespace WebLagerSystem
{
    public class ConsoleToHTMLPluklist
    {
        public static string PluklisteManager() {
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
    }
}
