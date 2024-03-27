using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OG.AIFileAnalyzer.Client;
using OG.AIFileAnalyzer.Client.Services.Analyzer;
using OG.AIFileAnalyzer.Client.Services.Historical;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configuration["backendUrl"]) });
builder.Services.AddSingleton<IAnalyzerService, AnalyzerService>();
builder.Services.AddSingleton<IHistoricalService, HistoricalService>();
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
