using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusiciansAPP.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

builder.SetUpLogging();
config.BindObjects();
services.AddAppCors();
services.AddAppServices(config);
services.AddDbServices(config);
services.AddControllers();

var app = builder.Build();
await app.SetUpDB();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();