using System.Text.Json;

namespace WebLagerSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var productList = new ProductList();

            app.MapGet("/", () =>
            {
                var products = productList.Products();
                var listItems = string.Join("\n", products.Select((p, idx) =>
                    $@"<li>
                        {p.Name} (Amount: {p.Amount})
                        <button type=""button"" class=""button is-danger is-small mb-2 ml-2 editButton"" data-idx=""{idx}"">edit product</button>
                        <form class=""editForm mt-2"" id=""editForm{idx}"" style=""display:none;"">
                            <input class=""input is-small mb-1"" type=""text"" name=""name"" value=""{p.Name}"" />
                            <input class=""input is-small mb-1"" type=""number"" name=""amount"" value=""{p.Amount}"" min=""0"" />
                            <input type=""hidden"" name=""idx"" value=""{idx}"" />
                            <button class=""button is-success is-small"" type=""submit"" name=""action"" value=""save"">Save</button>
                            <button class=""button is-danger is-small"" type=""button"" name=""action"" value=""delete"" onclick=""deleteProduct({idx})"">Delete</button>
                        </form>
                    </li>"
                ));

                var html = $@"<!DOCTYPE html>
                <html>
                <head>
                    <title>Hello</title>
                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
                </head>
                <body class=""is-flex is-align-items-center is-flex-direction-column"">
                    <div class=""is-justify-content-center is-align-items-center"">
                        <ul>
                            {listItems}
                        </ul>
                    </div>   
                    {WebLagerSystem.PluklisteWebSystem.pluklisteCreate()}
                </body>
                <script>
                    document.addEventListener(""DOMContentLoaded"", function() {{
                        document.querySelectorAll("".editButton"").forEach(function(btn) {{
                            btn.addEventListener(""click"", function() {{
                                var idx = btn.getAttribute(""data-idx"");
                                var form = document.getElementById(""editForm"" + idx);
                                if (form.style.display === ""none"") {{
                                    form.style.display = ""block"";
                                }} else {{
                                    form.style.display = ""none"";
                                }}
                            }});
                        }});
                        document.querySelectorAll("".editForm"").forEach(function(form) {{
                            form.addEventListener(""submit"", function(e) {{
                                e.preventDefault();
                                var formData = new FormData(form);
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
                        var formData = new FormData();
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
                </script>
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

                products[idx].Name = name;
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

            app.MapPost("/api/plukliste", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var filePath = Path.Combine(AppContext.BaseDirectory, "Pluklistfolder.json");
                await File.WriteAllTextAsync(filePath, body);
                context.Response.StatusCode = 200;
            });

            app.Run();
        }
        // In PluklisteWebSystem.cs
        public static string pluklisteCreate()
        {
            // return your HTML string here
            return "<div>Plukliste content</div>";
        }
    }

    public class ProductList : IProductList
    {
        private List<Product> products;
        private readonly string jsonPath;

        public ProductList()
        {
            jsonPath = Path.Combine(AppContext.BaseDirectory, "products.json");
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
                products = JsonSerializer.Deserialize<List<Product>>(json, options) ?? new List<Product>();
            }
            else
            {
                products = new List<Product>();
            }
        }

        public List<Product> Products()
        {
            return products;
        }

        public async Task SaveAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            await File.WriteAllTextAsync(jsonPath, JsonSerializer.Serialize(products, options));
        }
    }
}
