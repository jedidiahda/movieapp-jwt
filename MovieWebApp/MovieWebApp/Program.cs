using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieWebApp.Integration;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Middleware;
using MovieWebApp.Models;
using MovieWebApp.Repository;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MovieWebApp.JwtFeatures;
using Microsoft.Extensions.FileProviders;
using AutoMapper;
using MovieWebApp;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{

    var builder = WebApplication.CreateBuilder(args);
    var specificOrigins = "_specificOrigins";
    var connectionString = builder.Configuration.GetConnectionString("MovieDB");

    builder.Services.AddDbContext<MovieDbContext>(
        options => options.UseSqlServer(connectionString));

    //builder.Services.AddDbContext<MovieDbContextDerive>(
    //    options => options.UseSqlServer(connectionString));

    //builder.Services.AddMvc().AddXmlSerializerFormatters();
    builder.Services.AddMvc().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

    builder.Services.AddHttpClient();

    builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
    builder.Services.AddScoped<IDVDCatalogRepository, DVDCatalogRepository>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
    builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
    builder.Services.AddScoped<ICustomerDeliveryRepository, CustomerDeliveryRepository>();
    builder.Services.AddScoped<IRequestDVDRepository, RequestDVDRepository>();
    builder.Services.AddScoped<ILoggerManager, LoggerManager>();
    builder.Services.AddTransient<ExceptionMiddleware>();
    builder.Services.AddScoped<JwtHandler>();



    builder.Services.AddControllers()
            .AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: specificOrigins,
                          policy =>
                          {
                              policy
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                          });

    });



    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => {
        //options.SwaggerDoc("V1", new OpenApiInfo
        //{
        //    Version = "V1",
        //    Title = "WebAPI",
        //    Description = "Product WebAPI"
        //});
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Bearer Authentication with JWT Token",
            Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
        });
    });


    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
    builder.Services.AddSingleton(mapper);
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }



    app.UseHttpsRedirection();

    app.UseCors(specificOrigins);

    app.UseStaticFiles();
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
        RequestPath = new PathString("/Resources")
    });

    app.UseMiddleware<ExceptionMiddleware>();


    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
