using API.Configuration;
using API.Utilities;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationMediatR();
builder.Services.AddApplicationFluentValidation();
builder.Services.AddCarter();
builder.Services.AddApplicationJwtValidation(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddRedisConfiguration(builder.Configuration);
builder.Services.AddApplicationUtils();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AddConfigurationHelper.Initialize(builder.Configuration);
app.MapCarter();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();