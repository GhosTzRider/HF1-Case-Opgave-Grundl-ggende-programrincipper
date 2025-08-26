namespace WebLagerSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => Results.Content(
            @"<!DOCTYPE html>
            <html>
            <head>
                <title>Hello</title>
                <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bulma@1.0.4/css/bulma.min.css"">
            </head>
            <body>
                <div class=""is-flex is-justify-content-center is-align-items-center"">
                    <button class=""button is-primary is-inverted"">Click Me</button>
                </div>   
            </body>
            </html>", 
            "text/html"));
            app.Run();
        }
    }
}
