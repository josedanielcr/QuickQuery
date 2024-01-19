using API.Configuration;
using API.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDb(builder.Configuration);
builder.Services.AddApplicationMediatR();
builder.Services.AddApplicationFluentValidation();
builder.Services.AddCarter();
builder.Services.AddApplicationJwtValidation(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyDatabaseMigrations();
}

AddConfigurationHelper.Initialize(builder.Configuration);
app.MapCarter();
app.UseHttpsRedirection();
app.UseAuthentication();
app.Run();