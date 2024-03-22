using AutocompleteServiceAPI.Configuration;
using AutocompleteServiceAPI.Features;
using AutocompleteServiceAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddApplicationJwtValidation(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddCustomHttpClient();
builder.Services.AddScoped<HttpUtils>();
builder.Services.AddScoped<TrieInitialization>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var serviceProvider = builder.Services.BuildServiceProvider();
var trieInit = serviceProvider.GetService<TrieInitialization>();
trieInit.InitializeTrie(); 

app.UseHttpsRedirection();
app.MapHub<AutocompleteHub>("/api/autocomplete-hub");
app.UseAuthentication();
app.UseAuthorization();
app.Run();