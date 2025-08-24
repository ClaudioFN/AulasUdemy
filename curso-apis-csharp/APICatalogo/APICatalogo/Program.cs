using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// comentado na aula 72 builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// aula 72
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFiltercs));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

var valor1 = builder.Configuration["chave1"];
var secao1 = builder.Configuration["secao1:chave2"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection))
    );


builder.Services.AddScoped<ApiLoggingFilter>();

// 77
builder.Services.AddScoped<ICategoriaInterface, CategoriaRepository>();
// 80
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
// 86
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// 92
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// aula 71
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddTransient<IMeuServico, MeuServico>();

//101
builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

var app = builder.Build(); // a66 - da detalhes disso aqui como middlewares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
    //app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
