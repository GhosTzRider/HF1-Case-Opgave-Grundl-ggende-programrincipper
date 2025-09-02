using System.Text.Json;

namespace WebLagerSystem
{
    public class PluklisteWebSystem
    {
        public static string pluklisteCreate()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            // Add this API endpoint:
            app.MapPost("/api/plukliste", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                // Optionally validate/parse the JSON here
                var filePath1 = "Pluklistfolder.json";
                await File.WriteAllTextAsync(filePath1, body);
                context.Response.StatusCode = 200;
            });

            var HTMLPlukliste = $@"<!DOCTYPE html>

<html>
<head>
    <title>Plukliste</title>
    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
</head>
<body class=""is-flex is-align-items-center is-flex-direction-column"">
    <div class=""is-justify-content-center is-align-items-center"">
        <h1 class=""title"">Plukliste</h1>
        <form class=""editForm mt-2"" id=""editForm0"" style=""display:block;"">
            <p>Navn</p>
            <input class=""input is-small mb-1"" type=""text"" name=""PSN"" placeholder=""Navn"" />
            <p>Forsendelse</p>
            <input class=""input is-small mb-1"" type=""text"" name=""forsendelse"" placeholder=""Forsendelse"" />
            <p>Adresse</p>
            <input class=""input is-small mb-1"" type=""text"" name=""adresse"" placeholder=""Adresse"" />
        </form>
        <form id=""AddTemplateForm"">
            <div id=""´templateDropdowns"">
                <div class=""template-dropdown"">
                    <label>Choose a template:</label>
                    <select id=""Template"" class=""input is-small mb-1"">
                        <option value=""Print-Welcome"">Welcome</option>
                        <option value=""Print-Opgrade"">Opgrade</option>
                        <option value=""Print-Opsigelse"">Opsigelse</option>
                    </select>
                </div>
            </div>
        </form>
        <form id=""AddProductForm"">
            <div id=""productDropdowns"">
                <div class=""product-dropdown"">
                    <label>Choose a Product:</label>
                    <select name=""products"" class=""input is-small mb-1"">
                        <option value=""Netgear"">NETGEAR DOCSIS 3.1 (CM1000)</option>
                        <option value=""Triax"">Triax TD 241E</option>
                    </select>
                </div>
            </div>
            <br>
            <button id=""AddAnotherProductBtn"" class=""button is-success is-small"" type=""button"">Add Another Product</button>
            <br><br>
            <button class=""button is-success is-small"" type=""submit"">Submit Product</button>
        </form>
    </div>
    <script>
        document.getElementById('AddProductForm').addEventListener('submit', function(e) {{
            e.preventDefault();
            var form = e.target;
            var PSN = document.querySelector('input[name=""PSN""]').value;
            var forsendelse = document.querySelector('input[name=""forsendelse""]').value;
            var adresse = document.querySelector('input[name=""adresse""]').value;
            var template = document.getElementById('Template').value;
            var products = Array.from(form.querySelectorAll('select[name=""products""]')).map(select => select.value);
            products.push(template);
            var data = {{
                Name: PSN,
                Forsendelse: forsendelse,
                Adresse: adresse,
                Lines: products
            }};
            fetch('/api/plukliste', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify(data)
            }})
            .then(response => response.ok ? alert('Saved!') : alert('Error saving'))
            .catch(() => alert('Error saving'));
        }});
    </script>
    <script>
        document.addEventListener('DOMContentLoaded', function() {{
            var addBtn = document.getElementById('AddAnotherProductBtn');
            var dropdownsContainer = document.getElementById('productDropdowns');
            if (addBtn && dropdownsContainer) {{
                addBtn.addEventListener('click', function() {{
                    var dropdownHtml = `
                    <div class=""product-dropdown"">
                        <label>Choose a Product:</label>
                        <select name=""products"" class=""input is-small mb-1"">
                            <option value=""Netgear"">NETGEAR DOCSIS 3.1 (CM1000)</option>
                            <option value=""Triax"">Triax TD 241E</option>
                        </select>
                        <button class=""button is-danger is-small delete-product-btn"" type=""button"">Delete</button>
                    </div>
                    `;
                    dropdownsContainer.insertAdjacentHTML('beforeend', dropdownHtml);
                }});

                // Event delegation for delete buttons
                dropdownsContainer.addEventListener('click', function(e) {{
                    if (e.target && e.target.classList.contains('delete-product-btn')) {{
                        e.target.closest('.product-dropdown').remove();
                    }}
                }});
            }}
        }});
    </script>
</body>
</html>";
            app.RunAsync();
            return HTMLPlukliste;
        }

    }
}