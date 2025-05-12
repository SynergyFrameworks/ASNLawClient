using ASNLawClient.Components;
using ASNLawClient.Client.Services;
using ASNLawClient.Client.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using Microsoft.JSInterop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add base components
builder.Services.AddRazorComponents();

// Configure API settings from appsettings.json
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));

// Define a retry policy for HttpClient
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

// Register HttpClient with policy and fix base URL
builder.Services.AddHttpClient<ApiClient>((serviceProvider, client) => {
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

    // Ensure BaseUrl ends with slash
    var baseUrl = apiSettings.BaseUrl;
    if (!string.IsNullOrEmpty(baseUrl) && !baseUrl.EndsWith("/"))
        baseUrl += "/";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(apiSettings.TimeoutSeconds);
})
.AddPolicyHandler(retryPolicy);

builder.Services.AddHttpClient<AuthenticationService>((serviceProvider, client) => {
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

    // Ensure BaseUrl ends with slash
    var baseUrl = apiSettings.BaseUrl;
    if (!string.IsNullOrEmpty(baseUrl) && !baseUrl.EndsWith("/"))
        baseUrl += "/";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(apiSettings.TimeoutSeconds);
})
.AddPolicyHandler(retryPolicy);

// Register services
builder.Services.AddSingleton<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddSingleton<ToastService>();



// Add antiforgery services
builder.Services.AddAntiforgery();

// Add interactive components - important for .NET 9
var razorComponentsBuilder = builder.Services.AddRazorComponents();
razorComponentsBuilder.AddInteractiveServerComponents();
razorComponentsBuilder.AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// IMPORTANT: Ensure static files are mapped before adding components or routes
app.UseStaticFiles();  // Use static files middleware

// Add custom MIME types if needed
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".css"] = "text/css";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

// Add routing and antiforgery
app.UseRouting();
app.UseAntiforgery();

// Map Razor Components and Interactive WebAssembly in the correct order
var componentBuilder = app.MapRazorComponents<App>();
componentBuilder.AddInteractiveServerRenderMode();
componentBuilder.AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ASNLawClient.Client._Imports).Assembly);

app.MapRazorPages();

app.Run();
