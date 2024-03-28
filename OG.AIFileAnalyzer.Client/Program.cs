using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OG.AIFileAnalyzer.Client;
using OG.AIFileAnalyzer.Client.Services.Analyzer;
using OG.AIFileAnalyzer.Client.Services.Historical;
using Radzen;

// Entry point for the Blazor WebAssembly application setup.
var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

// Adds root components.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Sets up services.
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configuration["backendUrl"]) });
builder.Services.AddSingleton<IAnalyzerService, AnalyzerService>();
builder.Services.AddSingleton<IHistoricalService, HistoricalService>();
builder.Services.AddRadzenComponents();

// Executes the application asynchronously.
await builder.Build().RunAsync();
