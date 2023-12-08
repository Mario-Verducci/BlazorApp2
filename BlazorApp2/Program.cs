using BlazorApp2;
using BlazorApp2.Data;
using BlazorApp2.Data.Entities;
using BlazorApp2.Services;
using Bogus;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<AdressenService>();
builder.Services.AddSingleton<NetworkService>();
builder.Services.AddSqliteWasmDbContextFactory<ApplicationDbContext>
(
    opts => opts.UseSqlite("Data Source=things.sqlite3")
);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Ensure the database is created
var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContextFactor = services.GetRequiredService<ISqliteWasmDbContextFactory<ApplicationDbContext>>();
    var dbContext = await dbContextFactor.CreateDbContextAsync();
    await dbContext.Database.EnsureCreatedAsync();

    if (!await dbContext.Adressen.AnyAsync())
    {
        //Fill the database with some data
        Faker<Adresse> faker = new Faker<Adresse>("de");
        faker.RuleFor(a => a.Name1, f => f.Name.FullName());
        faker.RuleFor(a => a.Strasse, f => f.Address.StreetAddress());
        faker.RuleFor(a => a.Plz, f => f.Address.ZipCode());
        faker.RuleFor(a => a.Ort, f => f.Address.City());

        var fakeAdressen = faker.Generate(100);
        await dbContext.Adressen.AddRangeAsync(fakeAdressen);
        await dbContext.SaveChangesAsync();
    }
}

await host.RunAsync();