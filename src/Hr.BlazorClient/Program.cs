using Hr.BlazorClient;
using Hr.BlazorClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7224/";

builder.Services.AddSingleton<AuthState>();
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services
    .AddHttpClient<IHrApiClient, HrApiClient>(client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<AuthHeaderHandler>();

var host = builder.Build();

await host.Services.GetRequiredService<AuthState>().InitializeAsync();

await host.RunAsync();
