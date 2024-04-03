using AutocompleteServiceAPI.Configuration;
using AutocompleteServiceAPI.Features;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationJwtValidation(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddCustomHttpClient();
builder.Services.AddAppConfiguration();
builder.Services.AddApplicationMediatR();
builder.Services.AddCarter();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var serviceProvider = builder.Services.BuildServiceProvider();
var trieInit = serviceProvider.GetService<TrieInitialization>();
trieInit.InitializeTrie();
app.MapCarter();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();