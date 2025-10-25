using ApiCatalogoMinimal.ApiEndpoints;
using ApiCatalogoMinimal.AppServicesExtensions;
using ApiCatalogoMinimal.Context;
using ApiCatalogoMinimal.Models;
using ApiCatalogoMinimal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AppApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();



var app = builder.Build();

// Endpoint de Login
app.MapAutenticacaoEndpoints();

// Endpoints para o Categorias
// app.MapGet("/", () => "Catálogo de Produtos - 2025");
app.MapCategoriasEndpoints();

// Endpoints para Produtos
app.MapProdutosEndpoints();

// Equivalente ao Configure
var environment = app.Environment;

app.UseExceptionHandling(environment).UseSwaggerMiddleware().UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();