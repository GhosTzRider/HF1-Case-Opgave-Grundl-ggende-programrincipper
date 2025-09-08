using Plukliste;
using System.Globalization;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net; // Add this at the top

namespace WebLagerSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            var productList = new ProductList();

            // Make ProductList available to PluklisteWebSystem
            PluklisteWebSystem.ProductListInstance = productList;

            app.MapGet("/", () =>
            {
                var products = productList.Products();

                var tableRows = string.Join("\n", products.Select((p, idx) =>
                    $@"<tr>
                        <td>{WebUtility.HtmlEncode(p.Title)}</td>
                        <td>{p.Amount}</td>
                        <td>
                            <button type=""button"" class=""button is-small mb-2 ml-2 editButton"" data-idx=""{idx}"">Rediger produkt</button>
                        </td>
                        <td>
                        <form class=""editForm mt-2"" id=""editForm{idx}"" style=""display:none;"">
                            <input class=""input is-small mb-1"" type=""text"" name=""name"" value=""{WebUtility.HtmlEncode(p.Title)}"" />
                            <input class=""input is-small mb-1"" type=""number"" name=""amount"" value=""{p.Amount}"" min=""0"" />
                            <input type=""hidden"" name=""idx"" value=""{idx}"" />
                            <button class=""button is-success is-small"" type=""submit"" name=""action"" value=""save"">Save</button>
                            <button class=""button is-danger is-small"" type=""button"" name=""action"" value=""delete"" onclick=""deleteProduct({idx})"">Delete</button>
                        </form>
                        </td>
                    </tr>"
                ));

                var html = $@"<!DOCTYPE html>
                <html>
                <head>
                    <title>CMS system</title>
                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
                </head>
                <body>
                    <div class=""tabs is-centered"">
                        <ul>
                            <li class=""is-active"" data-tab=""tab-lagesystem""><a>Lagesystem</a></li>
                            <li data-tab=""tab-plukliste""><a>Create Plukliste</a></li>
                            <li data-tab=""tab-plukliste-manager""><a>Plukliste manager</a></li>
                        </ul>
                    </div>
                    <div id=""tab-lagesystem"" class=""tab-content"">
                        <div class=""columns is-gapless"">
                            <div class=""column is-half"">
                                <table class=""table is-striped is-fullwidth"">
                                    <thead>
                                        <tr>
                                            <th>Produkt Navn</th>
                                            <th>Antal</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {tableRows}
                                    </tbody>
                                </table>
                                <button class=""js-modal-trigger button is-primary"" data-target=""modal-js-example"">Tilf&oslashj Produkt</button>
                                {AddProduct.GetHtml()}
                            </div>
                        </div>
                    </div>
                    <div id=""tab-plukliste"" class=""tab-content"" style=""display:none;"">
                        <div class=""is-flex is-flex-direction-column column is-one-third"">
                            {PluklisteWebSystem.pluklisteCreate()}
                        </div>
                    </div>
                    <div id=""tab-plukliste-manager"" class=""tab-content"" style=""display:none;"">
    <div class=""is-flex is-flex-direction-column column"">
        <h1 class=""title"">Plukliste Manager</h1>
        <div id=""plukliste-manager-content"">
            {ConsoleToHTMLPluklist.PluklisteManager()}
        </div>
    </div>
</div>
                    <script>
                        // Tabs logic
                        document.addEventListener('DOMContentLoaded', function() {{
                            var tabs = document.querySelectorAll('.tabs ul li');
                            var tabContents = document.querySelectorAll('.tab-content');
                            tabs.forEach(function(tab) {{
                                tab.addEventListener('click', function() {{
                                    tabs.forEach(function(t) {{ t.classList.remove('is-active'); }});
                                    tabContents.forEach(function(tc) {{ tc.style.display = 'none'; }});
                                    tab.classList.add('is-active');
                                    var tabId = tab.getAttribute('data-tab');
                                    var content = document.getElementById(tabId);
                                    if(content) content.style.display = '';
                                }});
                            }});
                        }});

                        document.addEventListener(""DOMContentLoaded"", function() {{
                            document.querySelectorAll("".editButton"").forEach(function(btn) {{
                                btn.addEventListener(""click"", function() {{
                                    var idx = btn.getAttribute(""data-idx"");
                                    let form = document.getElementById(""editForm"" + idx);
                                    if (!form) return; 
                                    if (getComputedStyle(form).display === ""none"") {{
                                        form.style.display = ""block"";
                                    }} else {{
                                        form.style.display = ""none"";
                                    }}
                                }});
                            }});

                            document.querySelectorAll("".editForm"").forEach(function(form) {{
                                form.addEventListener(""submit"", function(e) {{
                                    e.preventDefault();
                                    
                                    let formData = new FormData(form);
                                    fetch(""/edit"", {{
                                        method: ""POST"",
                                        body: formData
                                    }})
                                    .then(response => response.json())
                                    .then(data => {{
                                        if (data.success) {{
                                            location.reload();
                                        }} else {{
                                            alert(""Failed to update product."");
                                        }}
                                    }});
                                }});
                            }});
                        }});

                        function deleteProduct(idx) {{
                            if (!confirm('Are you sure you want to delete this product?')) return;
                            let formData = new FormData();
                            formData.append('idx', idx);
                            fetch(""/delete"", {{
                                method: ""POST"",
                                body: formData
                            }})
                            .then(response => response.json())
                            .then(data => {{
                                if (data.success) {{
                                    location.reload();
                                }} else {{
                                    alert(""Failed to delete product."");
                                }}
                            }});
                        }}
                        function attachPluklisteBoxListener() {{
    const box = document.getElementById('plukliste-box');
    if (!box) return;
    box.addEventListener('click', function(e) {{
        if (e.target.getAttribute('data-action') === 'naeste' || e.target.getAttribute('data-action') === 'forrige') {{
            const action = e.target.getAttribute('data-action');
            let index = parseInt(box.getAttribute('data-index')) || 0;
            fetch('/plukliste/action', {{
                method: 'POST',
                headers: {{
                    'Content-Type': 'application/json'
                }},
                body: JSON.stringify({{
                    action: action, index: index
                }})
            }})
            .then(response => response.text())
            .then(html => {{
                document.getElementById('plukliste-manager-content').innerHTML = html;
                attachPluklisteBoxListener(); // Re-attach after update
            }});
        }}
    }});
}}

                    </script>

                </body>
                </html>";

                return Results.Content(html, "text/html");
            });

            // POST: Handle product edit
            app.MapPost("/edit", async (HttpRequest request) =>
            {
                productList.Reload(); // Always reload from file
                var form = await request.ReadFormAsync();
                if (!int.TryParse(form["idx"], out int idx)) return Results.Json(new { success = false });

                var name = form["name"].ToString();
                if (!int.TryParse(form["amount"], out int amount)) return Results.Json(new { success = false });

                var products = productList.Products();
                if (idx < 0 || idx >= products.Count) return Results.Json(new { success = false });

                products[idx].Title = name;
                products[idx].Amount = amount;

                await productList.SaveAsync(); // Always save to file

                return Results.Json(new { success = true });
            });

            // POST: Handle product delete
            app.MapPost("/delete", async (HttpRequest request) =>
            {
                productList.Reload(); // Always reload from file
                var form = await request.ReadFormAsync();
                if (!int.TryParse(form["idx"], out int idx)) return Results.Json(new { success = false });

                var products = productList.Products();
                if (idx < 0 || idx >= products.Count) return Results.Json(new { success = false });

                products.RemoveAt(idx);

                await productList.SaveAsync(); // Always save to file

                return Results.Json(new { success = true });
            });

            app.MapPost("/plukliste/action", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();

                var request = JsonSerializer.Deserialize<PluklisteActionRequest>(body);

                int index = request?.index ?? 0;
                string action = request?.action ?? "";

                var files = Directory.EnumerateFiles("export", "*.JSON")
                    .Select(f => f.Replace("export\\", ""))
                    .ToList();

                if (action == "naeste" && index < files.Count - 1)
                    index++;
                else if (action == "forrige" && index > 0)
                    index--;

                var html = WebLagerSystem.ConsoleToHTMLPluklist.PluklisteManager(index);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(html);
            });

            app.MapPost("/plukliste/afslut", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var request = JsonSerializer.Deserialize<PluklisteActionRequest>(body);

                int index = request?.index ?? 0;

                var exportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export");
                var importDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "import");
                var printDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "print");

                var files = Directory.EnumerateFiles(exportDir, "*.JSON")
                    .Select(f => Path.GetFileName(f))
                    .ToList();

                if (index < 0 || index >= files.Count)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid index");
                    return;
                }

                // Get the filename without the path
                string fileName = files[index];
                string sourceFile = Path.Combine(exportDir, fileName);
                string destFile = Path.Combine(importDir, fileName);

                // Load the Pluklist from the JSON file (so you can generate the HTML if needed)
                Pluklist? plukliste = null;
                try
                {
                    using var fileStream = File.OpenRead(sourceFile);
                    plukliste = await JsonSerializer.DeserializeAsync<Pluklist>(fileStream);
                }
                catch
                {
                    // Handle error if needed
                }

                // Generate HTML file as per your original console logic, if plukliste is available
                if (plukliste != null)
                {
                    string html = HTMLReader.ReplaceTagsInHTML(plukliste, "templateType"); // adjust 'templateType' if needed
                    string htmlFileName = Path.ChangeExtension(fileName, ".HTML");

                    if (!Directory.Exists(printDir))
                        Directory.CreateDirectory(printDir);

                    var htmlFilePath = Path.Combine(printDir, htmlFileName);

                    await File.WriteAllTextAsync(htmlFilePath, html);
                }

                // Move the JSON file to import folder (delete if exists)
                if (File.Exists(destFile))
                    File.Delete(destFile);

                File.Move(sourceFile, destFile);

                // Update the files list after moving
                files = Directory.EnumerateFiles(exportDir, "*.JSON")
                    .Select(f => Path.GetFileName(f))
                    .ToList();

                // Adjust index if necessary
                if (index >= files.Count)
                    index = files.Count - 1;

                // Return new plukliste manager HTML or a message if none left
                string htmlResult;

                if (files.Count == 0)
                {
                    htmlResult = "<div>Ingen pluklister fundet.</div>";
                }
                else
                {
                    htmlResult = WebLagerSystem.ConsoleToHTMLPluklist.PluklisteManager(index);
                }

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlResult);


                // Parse the incoming JSON
                var plukData = JsonSerializer.Deserialize<PluklisteRequest>(body);

                if (plukData == null || string.IsNullOrWhiteSpace(plukData.Name))
                    return Results.Json(new { success = false, error = "Missing name" });

                // Build the export object
                var exportObj = new
                {
                    Name = plukData.Name,
                    Forsendelse = plukData.Forsendelse,
                    Adresse = plukData.Adresse,
                    Lines = plukData.Lines.Select(line =>
                    {
                        var item = PluklisteWebSystem.ProductListInstance?.Products()
                            .FirstOrDefault(p => p.Title == line || p.ProductID == line);

                        return new
                        {
                            ProductID = item?.ProductID ?? "",
                            Title = item?.Title ?? line,
                            Type = item?.Type.ToString() ?? "",
                            Amount = item?.Amount ?? 1
                        };
                    }).ToList()
                };

                // Write to export folder in bin/debug (runtime)
                var exportDirRuntime = Path.Combine(AppContext.BaseDirectory, "export");
                Directory.CreateDirectory(exportDirRuntime);
                var fileName = $"{plukData.Name}_products.json";
                var filePathRuntime = Path.Combine(exportDirRuntime, fileName);

                // Write to export folder in project source (WebLagerSystem/export)
                var projectDir = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent;
                var exportDirProject = Path.Combine(projectDir?.FullName ?? "", "export");
                Directory.CreateDirectory(exportDirProject);
                var filePathProject = Path.Combine(exportDirProject, fileName);

                var json = JsonSerializer.Serialize(exportObj, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePathRuntime, json);
                await File.WriteAllTextAsync(filePathProject, json);

                return Results.Json(new { success = true });
            });

            app.MapPost("/add", async (HttpRequest request) =>
            {
                productList.Reload(); // Always reload from file
                var form = await request.ReadFormAsync();

                var id = form["id"].ToString();
                var name = form["name"].ToString();
                if (!int.TryParse(form["quantity"], out int quantity)) return Results.Json(new { success = false });

                // Assuming Product class has Id, Title, Amount
                productList.Products().Add(new Plukliste.Item
                {
                    ProductID = id,
                    Title = name,
                    Amount = quantity
                });

                await productList.SaveAsync(); // Always save to file

                return Results.Json(new { success = true });

            });

            app.Run();
        }

        public class PluklisteActionRequest
        {
            public int index { get; set; }
            public string action { get; set; }
        }

    }

    public class ProductList
    {
        private List<Plukliste.Item> products;
        private readonly string jsonPath;

        public ProductList()
        {
            jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.json");
            Reload();
        }

        public void Reload()
        {
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                products = JsonSerializer.Deserialize<List<Plukliste.Item>>(json, options) ?? new List<Plukliste.Item>();
            }
            else
            {
                products = new List<Plukliste.Item>();
            }
        }

        public List<Plukliste.Item> Products()
        {
            return products;
        }

        public async Task SaveAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            await File.WriteAllTextAsync(jsonPath, JsonSerializer.Serialize(products, options));
        }
    }


    public class PluklisteActionRequest
    {
        public int index { get; set; }
        public string action { get; set; }

    // Helper class for deserialization
    public class PluklisteRequest
    {
        public string Name { get; set; }
        public string Forsendelse { get; set; }
        public string Adresse { get; set; }
        public List<string> Lines { get; set; }

    }
}
