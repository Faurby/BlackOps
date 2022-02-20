using Microsoft.OpenApi.Models;
using MiniTwit.Server;
using Blazored.LocalStorage;
using MyApp.Server;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MiniTwitDatabaseSettings>(builder.Configuration.GetSection("MiniTwitDatabase"));

builder.Services.AddSingleton<MessagesService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<Utility>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApp.Server", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// app.Seed();

app.Run();
