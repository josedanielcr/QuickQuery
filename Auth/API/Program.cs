using QuickqueryAuthenticationAPI.Configuration;
using QuickqueryAuthenticationAPI.Database;
using QuickqueryAuthenticationAPI.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDb(builder.Configuration);
builder.Services.AddApplicationMediatR();
builder.Services.AddApplicationFluentValidation();
builder.Services.AddCarter();
builder.Services.AddApplicationCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyDatabaseMigrations();
}

AddConfigurationHelper.Initialize(builder.Configuration);
app.MapCarter();
app.UseCors("Policy");
app.UseHttpsRedirection();
app.Run();