using Microsoft.OpenApi.Models;
using MiniTwit.Server;
using Blazored.LocalStorage;
using MyApp.Server;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MiniTwitDatabaseSettings>(builder.Configuration.GetSection("MiniTwitDatabase"));

builder.Services.AddSingleton<IMessagesService, MessagesService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<Utility>();
builder.Services.AddSingleton<ILatestService, LatestService>();

builder.Services.AddSingleton<IMongoDatabase>(s =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")).GetDatabase("MiniTwit")
);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApp.Server", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

builder.WebHost.UseKestrel(options =>
    {
        options.Limits.MinRequestBodyDataRate = null;
        options.Limits.MinResponseDataRate = null;
        options.Limits.MaxRequestBodySize = null;

    });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    new Thread(async () =>
        {
            Thread.Sleep(4000);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:5142/sim/latest");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }).Start();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    new Thread(async () =>
        {
            Thread.Sleep(4000);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:80/sim/latest");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }).Start();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

// Prometheus stuff
app.UseMetricServer();
app.UseHttpMetrics();

Metrics.CreateCounter("sampleapp_ticks_total", "tik tok tik tok");


// app.UseEndpoints(endpoints =>
//     {
//         endpoints.MapControllers();
//         endpoints.MapMetrics();
//     });

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// app.Seed();

app.Run();

