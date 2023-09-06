using Wkhtmltopdf.NetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWkhtmltopdf();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/reporte-rotativa-with-htmlparam", (IGeneratePdf generatePdf, string html) =>
{
    var result = generatePdf.GetPDF(html);

    return result;
});

app.MapPost("/reporte-rotativa-with-template", (IGeneratePdf generatePdf) =>
{
    var htmlCode = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Template\\TemplateInscrito.html");

    htmlCode = htmlCode.Replace("@nombreInscrito", "Jorge Villaseca");

    var result = generatePdf.GetPDF(htmlCode);
    
    return result;
});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}