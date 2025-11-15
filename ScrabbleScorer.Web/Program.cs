using ScrabbleScorer.Core.Logic.Rules;
using ScrabbleScorer.Core.Repositories;
using ScrabbleScorer.Core.Services;
using ScrabbleScorer.Web.Components;
using ScrabbleScorer.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IPlacementRule, PlacementShouldBeNextToExistingPlacements>();
builder.Services.AddTransient<IPlacementRule, TileShouldBeEmpty>();
builder.Services.AddTransient<IPlacementRule, WordsCreatedShouldBeValid>();
builder.Services.AddTransient<IPlacementRule, WordShouldFitInsideBoard>();

builder.Services.AddSingleton<IWordRepository, WordRepository>();
builder.Services.AddSingleton<IGameService, GameService>();

builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
