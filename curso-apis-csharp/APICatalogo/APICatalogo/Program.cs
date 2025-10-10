using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.RateOptions;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

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

// 153
//var OrigensComAcessoPermitido = "_origensComAcessoPermitido";
//builder.Services.AddCors(options => options.AddPolicy(name: OrigensComAcessoPermitido, policy => {
//    policy.WithOrigins("http://www.apirequest.io");
//    }
//    ));
// 154

builder.Services.AddCors(options => options.AddDefaultPolicy( policy => {
    policy.WithOrigins("http://www.apirequest.io", "https://www.meu_site.com.br").WithMethods("GET", "POST").AllowAnyHeader().AllowCredentials();
}    ));

// 128
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var valor1 = builder.Configuration["chave1"];
var secao1 = builder.Configuration["secao1:chave2"];

var secretKey = builder.Configuration["JWT:SecretKey"] ??
    throw new ArgumentException("Invalid secret key!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Aula 127 - comentado na aula 142 pois ao tentar rodar o programa, trava por causa desses 2
//builder.Services.AddAuthorization();
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { Version = "v1"
    , Title = "APICatalogo"
    , Description = "API para Desenvolvimento de conhecimentos!"
    , TermsOfService = new Uri("https://www.meusite.com.br/terms")
    
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
    // 173
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "Claudio"));

    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    options.AddPolicy("ExclusivePolicyAdminOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(
        claim => claim.Type == "id" && claim.Value == "Claudio") || context.User.IsInRole("SuperAdmin")));
});

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection))
    );

// 162 
var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

// 159
builder.Services.AddRateLimiter(rateLimitOptions =>
{
    rateLimitOptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
    {
        options.PermitLimit = myOptions.PermitLimit; // 1;
        options.Window = TimeSpan.FromSeconds(myOptions.Window); // (5);
        options.QueueLimit = myOptions.QueueLimit; //2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    rateLimitOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// 161
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpcontext.User.Identity?.Name ?? httpcontext.Request.Headers.Host.ToString(),
        factory: partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 5,
            QueueLimit = 0,
            Window = TimeSpan.FromSeconds(10)
        }
        ));
});

// 166 
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    // 167
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader(),
        new UrlSegmentApiVersionReader());
}).AddApiExplorer( o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<ApiLoggingFilter>();
// 77
builder.Services.AddScoped<ICategoriaInterface, CategoriaRepository>();
// 80
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
// 86
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// 92
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// 138 
builder.Services.AddScoped<ITokenService, TokenService>();


// aula 71
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddTransient<IMeuServico, MeuServico>();

//101
builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

// 192
builder.Services.AddMemoryCache();

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
// 153
app.UseRouting();
// 159
app.UseRateLimiter();

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
