using System.Reflection;
using Asp.Versioning.Builder;
using CJDropship.API.Apis;
using CJDropship.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapControllers();
app.UseStatusCodePages();
app.MapCJApiV1();

app.UseDefaultOpenApi();
app.Run();
