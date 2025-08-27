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
                var listItems = string.Join("\n", products.Select(p => $"<li>{p.Name} (Amount: {p.Amount})</li>"));
                Console.WriteLine();

                var html = $@"<!DOCTYPE html>
                <html>
                <head>
                    <title>Hello</title>
                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
                </head>
                <body>
                    <div class=""is-flex is-justify-content-center is-align-items-center"">
                        <ul>
                            {listItems}
                        </ul>
                        <button class=""button is-primary is-inverted"">Click Me</button>
                    </div>   
                </body>
                </html>";

                return Results.Content(html, "text/html");
            });

            app.Run();
        }
    }

    public class ProductList : IProductList
    {
        private readonly List<Product> products;

        public ProductList()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "products.json");
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
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
    }
}
